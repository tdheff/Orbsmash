using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Nez;
using Nez.Particles;


namespace Handy.Components
{
	public class ParticleEmitterComponent : RenderableComponent
	{
		public override RectangleF bounds { get { return _bounds; } }
		public RectangleF Bounds
		{
			get => _bounds;
			set => _bounds = value;
		}

		public bool isPaused { get { return _isPaused; } }
		public bool isPlaying { get { return _active && !_isPaused; } }
		public bool isStopped { get { return !_active && !_isPaused; } }
		public bool isEmitting { get { return _emitting; } }
		public float elapsedTime { get { return _elapsedTime; } }

		/// <summary>
		/// convenience method for setting ParticleEmitterConfig.simulateInWorldSpace. If true, particles will simulate in world space. ie when the
		/// parent Transform moves it will have no effect on any already active Particles.
		/// </summary>
		public bool simulateInWorldSpace { set { _emitterConfig.simulateInWorldSpace = value; } }

		/// <summary>
		/// config object with various properties to deal with particle collisions
		/// </summary>
		public ParticleCollisionConfig collisionConfig;

		/// <summary>
		/// event that's going to be called when particles count becomes 0 after stopping emission.
		/// emission can stop after either we stop it manually or when we run for entire duration specified in ParticleEmitterConfig.
		/// </summary>
		public event Action<ParticleEmitterComponent> onAllParticlesExpired;
		/// <summary>
		/// event that's going to be called when emission is stopped due to reaching duration specified in ParticleEmitterConfig
		/// </summary>
		public event Action<ParticleEmitterComponent> onEmissionDurationReached;

		/// <summary>
		/// keeps track of how many particles should be emitted
		/// </summary>
		public float _emitCounter;

		/// <summary>
		/// tracks the elapsed time of the emitter
		/// </summary>
		public float _elapsedTime;

		public bool _active = false;
		public bool _isPaused;

		/// <summary>
		/// if the emitter is emitting this will be true. Note that emitting can be false while particles are still alive. emitting gets set
		/// to false and then any live particles are allowed to finish their lifecycle.
		/// </summary>
		public bool _emitting;
		public List<Particle> _particles;
		public bool _playOnAwake;
		public ParticleEmitterConfig _emitterConfig;


		public ParticleEmitterComponent( ParticleEmitterConfig emitterConfig, bool playOnAwake = true )
		{
			_emitterConfig = emitterConfig;
			_playOnAwake = playOnAwake;
			_particles = new List<Particle>( (int)_emitterConfig.maxParticles );
			Pool<Particle>.warmCache( (int)_emitterConfig.maxParticles );
			
			// set some sensible defaults
			collisionConfig.elasticity = 0.5f;
			collisionConfig.friction = 0.5f;
			collisionConfig.collidesWithLayers = Physics.allLayers;
			collisionConfig.gravity = _emitterConfig.gravity;
			collisionConfig.lifetimeLoss = 0f;
			collisionConfig.minKillSpeedSquared = float.MinValue;
			collisionConfig.radiusScale = 0.8f;

			init();
		}


		/// <summary>
		/// creates the Batcher and loads the texture if it is available
		/// </summary>
		void init()
		{
			// prep our custom BlendState and set the Material with it
			var blendState = new BlendState();
			blendState.ColorSourceBlend = blendState.AlphaSourceBlend = _emitterConfig.blendFuncSource;
			blendState.ColorDestinationBlend = blendState.AlphaDestinationBlend = _emitterConfig.blendFuncDestination;

			material = new Material( blendState );
		}


		#region Component/RenderableComponent

		public override void onAddedToEntity()
		{
			if( _playOnAwake )
				play();
		}

		public override void render( Graphics graphics, Camera camera )
		{
			// we still render when we are paused
			if( !_active && !_isPaused )
				return;

			var rootPosition = entity.transform.position + _localOffset;

			// loop through all the particles updating their location and color
			for( var i = 0; i < _particles.Count; i++ )
			{
				var currentParticle = _particles[i];
				var pos = _emitterConfig.simulateInWorldSpace ? currentParticle.spawnPosition : rootPosition;

				if (_emitterConfig.subtexture == null)
				{
					currentParticle._radius = 100;
					graphics.batcher.draw(graphics.pixelTexture, pos + currentParticle.position, currentParticle.color,
						currentParticle.rotation, Vector2.One, currentParticle.particleSize * 0.5f, SpriteEffects.None,
						layerDepth);
				}
				else
				{
					graphics.batcher.draw(_emitterConfig.subtexture, pos + currentParticle.position,
						currentParticle.color, currentParticle.rotation, _emitterConfig.subtexture.center,
						currentParticle.particleSize / _emitterConfig.subtexture.sourceRect.Width, SpriteEffects.None,
						layerDepth);
				}
			}
		}

		#endregion


		/// <summary>
		/// removes all particles from the particle emitter
		/// </summary>
		public void clear()
		{
			for( var i = 0; i < _particles.Count; i++ )
				Pool<Particle>.free( _particles[i] );
			_particles.Clear();
		}


		/// <summary>
		/// plays the particle emitter
		/// </summary>
		public void play()
		{
			// if we are just unpausing, we only toggle flags and we dont mess with any other parameters
			if( _isPaused )
			{
				_active = true;
				_isPaused = false;
				return;
			}

			_active = true;
			_emitting = true;
			_elapsedTime = 0;
			_emitCounter = 0;
		}


		/// <summary>
		/// stops the particle emitter
		/// </summary>
		public void stop()
		{
			_active = false;
			_isPaused = false;
			_elapsedTime = 0;
			_emitCounter = 0;
			clear();
		}


		/// <summary>
		/// pauses the particle emitter
		/// </summary>
		public void pause()
		{
			_isPaused = true;
			_active = false;
		}


		/// <summary>
		/// resumes emission of particles.
		/// this is possible only if stop() wasn't called and emission wasn't stopped due to duration
		/// </summary>
		public void resumeEmission()
		{
			if( isStopped || ( _emitterConfig.duration != -1 && _emitterConfig.duration < _elapsedTime ) )
				return;

			_emitting = true;
		}


		/// <summary>
		/// pauses emission of particles while allowing existing particles to expire
		/// </summary>
		public void pauseEmission()
		{
			_emitting = false;
		}


		/// <summary>
		/// manually emit some particles
		/// </summary>
		/// <param name="count">Count.</param>
		public void emit( int count )
		{
			var rootPosition = entity.transform.position + _localOffset;

			init();
			_active = true;
			for( var i = 0; i < count; i++ )
				addParticle( rootPosition );
		}


		/// <summary>
		/// adds a Particle to the emitter
		/// </summary>
		public void addParticle( Vector2 position )
		{
			// take the next particle out of the particle pool we have created and initialize it
			var particle = Pool<Particle>.obtain();
			particle.initialize( _emitterConfig, position );
			_particles.Add( particle );
		}

	}
}

