//using Handy.Components;
//using Nez;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//
//namespace Handy.Systems
//{
//    class SpriteAnimationSystem : EntitySystem
//    {
//        public SpriteAnimationSystem() : this(new Matcher().all(
//                typeof(AnimatedSpriteComponent<>)
//            ))
//        { }
//
//        public SpriteAnimationSystem(Matcher matcher) : base(matcher)
//        { }
//
//        protected override void process(List<Entity> entities)
//        {
//            foreach (var entity in entities)
//            {
//                var s = entity.getComponent<AnimatedSpriteComponent>();
//
//                if (s.currentAnimation == null || !isPlaying)
//                    return;
//
//                // handle delay
//                if (!_delayComplete && _elapsedDelay < _currentAnimation.delay)
//                {
//                    _elapsedDelay += Time.deltaTime;
//                    if (_elapsedDelay >= _currentAnimation.delay)
//                        _delayComplete = true;
//
//                    return;
//                }
//
//                // count backwards if we are going in reverse
//                if (_isReversed)
//                    _totalElapsedTime -= Time.deltaTime;
//                else
//                    _totalElapsedTime += Time.deltaTime;
//
//
//                _totalElapsedTime = Mathf.clamp(_totalElapsedTime, 0f, _currentAnimation.totalDuration);
//                _completedIterations = Mathf.floorToInt(_totalElapsedTime / _currentAnimation.iterationDuration);
//                _isLoopingBackOnPingPong = false;
//
//
//                // handle ping pong loops. if loop is false but pingPongLoop is true we allow a single forward-then-backward iteration
//                if (_currentAnimation.pingPong)
//                {
//                    if (_currentAnimation.loop || _completedIterations < 2)
//                        _isLoopingBackOnPingPong = _completedIterations % 2 != 0;
//                }
//
//
//                var elapsedTime = 0f;
//                if (_totalElapsedTime < _currentAnimation.iterationDuration)
//                {
//                    elapsedTime = _totalElapsedTime;
//                }
//                else
//                {
//                    elapsedTime = _totalElapsedTime % _currentAnimation.iterationDuration;
//
//                    // if we arent looping and elapsedTime is 0 we are done. Handle it appropriately
//                    if (!_currentAnimation.loop && elapsedTime == 0)
//                    {
//                        // the animation is done so fire our event
//                        if (onAnimationCompletedEvent != null)
//                            onAnimationCompletedEvent(_currentAnimationKey);
//
//                        isPlaying = false;
//
//                        switch (_currentAnimation.completionBehavior)
//                        {
//                            case AnimationCompletionBehavior.RemainOnFinalFrame:
//                                return;
//                            case AnimationCompletionBehavior.RevertToFirstFrame:
//                                setSubtexture(_currentAnimation.frames[0]);
//                                return;
//                            case AnimationCompletionBehavior.HideSprite:
//                                _subtexture = null;
//                                _currentAnimation = null;
//                                return;
//                        }
//                    }
//                }
//
//
//                // if we reversed the animation and we reached 0 total elapsed time handle un-reversing things and loop continuation
//                if (_isReversed && _totalElapsedTime <= 0)
//                {
//                    _isReversed = false;
//
//                    if (_currentAnimation.loop)
//                    {
//                        _totalElapsedTime = 0f;
//                    }
//                    else
//                    {
//                        // the animation is done so fire our event
//                        if (onAnimationCompletedEvent != null)
//                            onAnimationCompletedEvent(_currentAnimationKey);
//
//                        isPlaying = false;
//                        return;
//                    }
//                }
//
//                // time goes backwards when we are reversing a ping-pong loop
//                if (_isLoopingBackOnPingPong)
//                    elapsedTime = _currentAnimation.iterationDuration - elapsedTime;
//
//
//                // fetch our desired frame
//                var desiredFrame = Mathf.floorToInt(elapsedTime / _currentAnimation.secondsPerFrame);
//                if (desiredFrame != currentFrame)
//                {
//                    currentFrame = desiredFrame;
//                    setSubtexture(_currentAnimation.frames[currentFrame]);
//                    handleFrameChanged();
//
//                    // ping-pong needs special care. we don't want to double the frame time when wrapping so we man-handle the totalElapsedTime
//                    if (_currentAnimation.pingPong && (currentFrame == 0 || currentFrame == _currentAnimation.frames.Count - 1))
//                    {
//                        if (_isReversed)
//                            _totalElapsedTime -= _currentAnimation.secondsPerFrame;
//                        else
//                            _totalElapsedTime += _currentAnimation.secondsPerFrame;
//                    }
//                }
//            }
//        }
//}

