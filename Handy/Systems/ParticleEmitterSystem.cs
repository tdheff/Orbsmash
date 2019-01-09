using System.Collections.Generic;
using Handy.Components;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Particles;

namespace Handy.Systems
{
    public class ParticleEmitterSystem : EntitySystem
    {
	    public ParticleEmitterSystem() : base(new Matcher().all(typeof(ParticleEmitterComponent))) { }
	    
        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var particleEmitter = entity.getComponent<ParticleEmitterComponent>();
                if( particleEmitter._isPaused )
				return;

				// prep data for the particle.update method
				var rootPosition = entity.transform.position + particleEmitter.localOffset;
				
				// if the emitter is active and the emission rate is greater than zero then emit particles
				if( particleEmitter._active && particleEmitter._emitterConfig.emissionRate > 0 )
				{
					if( particleEmitter._emitting )
					{
						var rate = 1.0f / particleEmitter._emitterConfig.emissionRate;
	
						if( particleEmitter._particles.Count < particleEmitter._emitterConfig.maxParticles )
							particleEmitter._emitCounter += Time.deltaTime;
	
						while( particleEmitter._particles.Count < particleEmitter._emitterConfig.maxParticles && particleEmitter._emitCounter > rate )
						{
							particleEmitter.addParticle( rootPosition );
							particleEmitter._emitCounter -= rate;
						}
	
						particleEmitter._elapsedTime += Time.deltaTime;
	
						if( particleEmitter._emitterConfig.duration != -1 && particleEmitter._emitterConfig.duration < particleEmitter._elapsedTime )
						{
							// when we hit our duration we dont emit any more particles
							particleEmitter._emitting = false;
						}
					}
	
					// once all our particles are done we stop the emitter
					if( !particleEmitter._emitting && particleEmitter._particles.Count == 0 )
					{
						particleEmitter.stop();
					}
				}
	
				var min = new Vector2( float.MaxValue, float.MaxValue );
				var max = new Vector2( float.MinValue, float.MinValue );
				var maxParticleSize = float.MinValue;
	
				// loop through all the particles updating their location and color
				for( var i = particleEmitter._particles.Count - 1; i >= 0; i-- )
				{
					// get the current particle and update it
					var currentParticle = particleEmitter._particles[i];
	
					// if update returns true that means the particle is done
					if( currentParticle.update( particleEmitter._emitterConfig, ref particleEmitter.collisionConfig, rootPosition ) )
					{
						Pool<Particle>.free( currentParticle );
						particleEmitter._particles.RemoveAt( i );
					}
					else
					{
						// particle is good. collect min/max positions for the bounds
						var pos = particleEmitter._emitterConfig.simulateInWorldSpace ? currentParticle.spawnPosition : rootPosition;
						pos += currentParticle.position;
						Vector2.Min( ref min, ref pos, out min );
						Vector2.Max( ref max, ref pos, out max );
						maxParticleSize = System.Math.Max( maxParticleSize, currentParticle.particleSize );
					}
				}
	
				var bounds = particleEmitter.Bounds;
				bounds.location = min;
				bounds.width = max.X - min.X;
				bounds.height = max.Y - min.Y;
				particleEmitter.Bounds = bounds;
	
				if( particleEmitter._emitterConfig.subtexture == null )
				{
					particleEmitter.Bounds.inflate( 1 * maxParticleSize, 1 * maxParticleSize );
				}
				else
				{
					maxParticleSize /= particleEmitter._emitterConfig.subtexture.sourceRect.Width;
					particleEmitter.Bounds.inflate( particleEmitter._emitterConfig.subtexture.sourceRect.Width * maxParticleSize, particleEmitter._emitterConfig.subtexture.sourceRect.Height * maxParticleSize );
				}
            }
        }
    }
}