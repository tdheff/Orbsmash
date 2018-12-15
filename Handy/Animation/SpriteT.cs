using System;
using System.Collections.Generic;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace Handy.Animation
{
	/// <summary>
	/// Sprite class handles the display and animation of a sprite. It uses a suggested Enum as a key (you can use an int as well if you
	/// prefer). If you do use an Enum it is recommended to pass in a IEqualityComparer when using an enum like CoreEvents does. See also
	/// the EnumEqualityComparerGenerator.tt T4 template for automatically generating the IEqualityComparer.
	/// </summary>
	public class Sprite<TEnum> : Nez.Sprites.Sprite where TEnum : struct, IComparable, IFormattable
	{
        // TODO - actions!
		// public event Action<TEnum> onAnimationCompletedEvent;
		public bool IsPlaying;
		public int CurrentFrame;

		Dictionary<TEnum, SpriteAnimation> _animations;

		// playback state
		public SpriteAnimation CurrentAnimation;
		public TEnum CurrentAnimationKey;
		public float TotalElapsedTime;
		public float ElapsedDelay;
		public int CompletedIterations;
		public bool DelayComplete;
		public bool IsReversed;
		public bool IsLoopingBackOnPingPong;

		public Subtexture Subtexture
		{
			get => _subtexture;
			set => _subtexture = value;
		}


		/// <summary>
		/// beware the beast man! If you use this constructor you must set the subtexture or set animations so that this sprite has proper bounds
		/// when the Scene is running.
		/// </summary>
		/// <param name="customComparer">Custom comparer.</param>
		public Sprite( IEqualityComparer<TEnum> customComparer = null ) : base( Graphics.instance.pixelTexture )
		{
			_animations = new Dictionary<TEnum, SpriteAnimation>( customComparer );
		}


		public Sprite( IEqualityComparer<TEnum> customComparer, Subtexture subtexture ) : base( subtexture )
		{
			_animations = new Dictionary<TEnum, SpriteAnimation>( customComparer );
		}


		/// <summary>
		/// Sprite needs a Subtexture at constructor time so that it knows how to size itself
		/// </summary>
		/// <param name="subtexture">Subtexture.</param>
		public Sprite( Subtexture subtexture ) : this( null, subtexture )
		{ }


		/// <summary>
		/// Sprite needs a Subtexture at constructor time so the first frame of the passed in animation will be used for this constructor
		/// </summary>
		/// <param name="animationKey">Animation key.</param>
		/// <param name="animation">Animation.</param>
		public Sprite( TEnum animationKey, SpriteAnimation animation ) : this( null, animation.frames[0] )
		{
			addAnimation( animationKey, animation );
		}

		public Sprite<TEnum> addAnimation( TEnum key, SpriteAnimation animation )
		{
			// if we have no subtexture use the first frame we find
			if( _subtexture == null && animation.frames.Count > 0 )
				setSubtexture( animation.frames[0] );
			_animations[key] = animation;

			return this;
		}


		public SpriteAnimation getAnimation( TEnum key )
		{
			Assert.isTrue( _animations.ContainsKey( key ), "{0} is not present in animations", key );
			return _animations[key];
		}


		#region Playback

		/// <summary>
		/// plays the animation at the given index. You can cache the indices by calling animationIndexForAnimationName.
		/// </summary>
		/// <param name="animationKey">Animation key.</param>
		/// <param name="startFrame">Start frame.</param>
		public SpriteAnimation play( TEnum animationKey, int startFrame = 0 )
		{
			Assert.isTrue( _animations.ContainsKey( animationKey ), "Attempted to play an animation that doesnt exist" );

			var animation = _animations[animationKey];
			animation.prepareForUse();

			CurrentAnimationKey = animationKey;
			CurrentAnimation = animation;
			IsPlaying = true;
			IsReversed = false;
			CurrentFrame = startFrame;
			setSubtexture( CurrentAnimation.frames[CurrentFrame] );

			TotalElapsedTime = (float)startFrame * CurrentAnimation.secondsPerFrame;
			return animation;
		}


		public bool isAnimationPlaying( TEnum animationKey )
		{
			return CurrentAnimation != null && CurrentAnimationKey.Equals( animationKey );
		}


		public void pause()
		{
			IsPlaying = false;
		}


		public void unPause()
		{
			IsPlaying = true;
		}


		public void reverseAnimation()
		{
			IsReversed = !IsReversed;
		}


		public void stop()
		{
			IsPlaying = false;
			CurrentAnimation = null;
		}

		#endregion


		public void handleFrameChanged()
		{
			// TODO: add animation frame triggers
		}

	}
}

