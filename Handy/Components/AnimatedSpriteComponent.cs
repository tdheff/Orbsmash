//using Nez;
//using Nez.Sprites;
//using Nez.Textures;
//using System;
//using System.Collections.Generic;
//
//namespace Handy.Components
//{
//    /// <summary>
//	/// Sprite class handles the display and animation of a sprite. It uses a suggested Enum as a key (you can use an int as well if you
//	/// prefer). If you do use an Enum it is recommended to pass in a IEqualityComparer when using an enum like CoreEvents does. See also
//	/// the EnumEqualityComparerGenerator.tt T4 template for automatically generating the IEqualityComparer.
//	/// </summary>
//	public class AnimatedSpriteComponent<TEnum> : Sprite where TEnum : struct, IComparable, IFormattable
//    {
//        public event Action<TEnum> onAnimationCompletedEvent;
//        public bool isPlaying { get; private set; }
//        public int currentFrame { get; private set; }
//
//        /// <summary>
//        /// gets/sets the currently playing animation
//        /// </summary>
//        /// <value>The current animation.</value>
//        public TEnum currentAnimation
//        {
//            get { return currentAnimationKey; }
//            set { play(value); }
//        }
//
//        Dictionary<TEnum, SpriteAnimation> _animations;
//
//        // playback state
//        public SpriteAnimation currentSpriteAnimation;
//        public TEnum currentAnimationKey;
//        public float totalElapsedTime;
//        public float elapsedDelay;
//        public int completedIterations;
//        public bool delayComplete;
//        public bool isReversed;
//        public bool isLoopingBackOnPingPong;
//
//
//        /// <summary>
//        /// beware the beast man! If you use this constructor you must set the subtexture or set animations so that this sprite has proper bounds
//        /// when the Scene is running.
//        /// </summary>
//        /// <param name="customComparer">Custom comparer.</param>
//        public AnimatedSpriteComponent(IEqualityComparer<TEnum> customComparer = null) : base(Graphics.instance.pixelTexture)
//        {
//            _animations = new Dictionary<TEnum, SpriteAnimation>(customComparer);
//        }
//
//
//        public AnimatedSpriteComponent(IEqualityComparer<TEnum> customComparer, Subtexture subtexture) : base(subtexture)
//        {
//            _animations = new Dictionary<TEnum, SpriteAnimation>(customComparer);
//        }
//
//
//        /// <summary>
//        /// Sprite needs a Subtexture at constructor time so that it knows how to size itself
//        /// </summary>
//        /// <param name="subtexture">Subtexture.</param>
//        public AnimatedSpriteComponent(Subtexture subtexture) : this(null, subtexture)
//        { }
//
//
//        /// <summary>
//        /// Sprite needs a Subtexture at constructor time so the first frame of the passed in animation will be used for this constructor
//        /// </summary>
//        /// <param name="animationKey">Animation key.</param>
//        /// <param name="animation">Animation.</param>
//        public AnimatedSpriteComponent(TEnum animationKey, SpriteAnimation animation) : this(null, animation.frames[0])
//        {
//            addAnimation(animationKey, animation);
//        }
//
//        public AnimatedSpriteComponent<TEnum> addAnimation(TEnum key, SpriteAnimation animation)
//        {
//            // if we have no subtexture use the first frame we find
//            if (_subtexture == null && animation.frames.Count > 0)
//                setSubtexture(animation.frames[0]);
//            _animations[key] = animation;
//
//            return this;
//        }
//
//
//        public SpriteAnimation getAnimation(TEnum key)
//        {
//            Assert.isTrue(_animations.ContainsKey(key), "{0} is not present in animations", key);
//            return _animations[key];
//        }
//
//
//        #region Playback
//
//        /// <summary>
//        /// plays the animation at the given index. You can cache the indices by calling animationIndexForAnimationName.
//        /// </summary>
//        /// <param name="animationKey">Animation key.</param>
//        /// <param name="startFrame">Start frame.</param>
//        public SpriteAnimation play(TEnum animationKey, int startFrame = 0)
//        {
//            Assert.isTrue(_animations.ContainsKey(animationKey), "Attempted to play an animation that doesnt exist");
//
//            var animation = _animations[animationKey];
//            animation.prepareForUse();
//
//            currentAnimationKey = animationKey;
//            currentSpriteAnimation = animation;
//            isPlaying = true;
//            isReversed = false;
//            currentFrame = startFrame;
//            setSubtexture(currentSpriteAnimation.frames[currentFrame]);
//
//            totalElapsedTime = (float)startFrame * currentSpriteAnimation.secondsPerFrame;
//            return animation;
//        }
//
//
//        public bool isAnimationPlaying(TEnum animationKey)
//        {
//            return currentSpriteAnimation != null && currentAnimationKey.Equals(animationKey);
//        }
//
//
//        public void pause()
//        {
//            isPlaying = false;
//        }
//
//
//        public void unPause()
//        {
//            isPlaying = true;
//        }
//
//
//        public void reverseAnimation()
//        {
//            isReversed = !isReversed;
//        }
//
//
//        public void stop()
//        {
//            isPlaying = false;
//            currentAnimation = null;
//        }
//
//        #endregion
//
//
//        void handleFrameChanged()
//        {
//            // TODO: add animation frame triggers
//        }
//
//    }
//}
