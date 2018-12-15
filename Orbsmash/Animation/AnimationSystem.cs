using System;
using System.Collections.Generic;
using Nez;
using Nez.Sprites;
using Orbsmash.Animation;

namespace Handy.Systems
{
    public class AnimationSystem : EntitySystem
    {
	    public AnimationSystem() : base(new Matcher().all(typeof(SpriteComponent))) { }
	    
	    protected override void process(List<Entity> entities)
	    {
		    foreach (var entity in entities)
		    {
			    var sprite = entity.getComponent<SpriteComponent>();
			    
			    if( sprite.CurrentAnimation == null || !sprite.IsPlaying )
					return;

				// handle delay
				if( !sprite.DelayComplete && sprite.ElapsedDelay < sprite.CurrentAnimation.delay )
				{
					sprite.ElapsedDelay += Time.deltaTime;
					if( sprite.ElapsedDelay >= sprite.CurrentAnimation.delay )
						sprite.DelayComplete = true;
	
					return;
				}
	
				// count backwards if we are going in reverse
				if( sprite.IsReversed )
					sprite.TotalElapsedTime -= Time.deltaTime;
				else
					sprite.TotalElapsedTime += Time.deltaTime;
	
	
				sprite.TotalElapsedTime = Mathf.clamp( sprite.TotalElapsedTime, 0f, sprite.CurrentAnimation.totalDuration );
				sprite.CompletedIterations = Mathf.floorToInt( sprite.TotalElapsedTime / sprite.CurrentAnimation.iterationDuration );
				sprite.IsLoopingBackOnPingPong = false;
	
	
				// handle ping pong loops. if loop is false but pingPongLoop is true we allow a single forward-then-backward iteration
				if( sprite.CurrentAnimation.pingPong )
				{
					if( sprite.CurrentAnimation.loop || sprite.CompletedIterations < 2 )
						sprite.IsLoopingBackOnPingPong = sprite.CompletedIterations % 2 != 0;
				}
	
	
				var elapsedTime = 0f;
				if( sprite.TotalElapsedTime < sprite.CurrentAnimation.iterationDuration )
				{
					elapsedTime = sprite.TotalElapsedTime;
				}
				else
				{
					elapsedTime = sprite.TotalElapsedTime % sprite.CurrentAnimation.iterationDuration;
	
					// if we arent looping and elapsedTime is 0 we are done. Handle it appropriately
					if( !sprite.CurrentAnimation.loop && elapsedTime == 0 )
					{
						// the animation is done so fire our event
						// TODO - events
						/*
						if( onAnimationCompletedEvent != null )
							onAnimationCompletedEvent( sprite.CurrentAnimationKey );
							*/
	
						sprite.IsPlaying = false;
	
						switch( sprite.CurrentAnimation.completionBehavior )
						{
							case AnimationCompletionBehavior.RemainOnFinalFrame:
								return;
							case AnimationCompletionBehavior.RevertToFirstFrame:
								sprite.setSubtexture( sprite.CurrentAnimation.frames[0] );
								return;
							case AnimationCompletionBehavior.HideSprite:
								sprite.Subtexture = null;
								sprite.CurrentAnimation = null;
								return;
						}
					}
				}
	
	
				// if we reversed the animation and we reached 0 total elapsed time handle un-reversing things and loop continuation
				if( sprite.IsReversed && sprite.TotalElapsedTime <= 0 )
				{
					sprite.IsReversed = false;
	
					if( sprite.CurrentAnimation.loop )
					{
						sprite.TotalElapsedTime = 0f;
					}
					else
					{
						// the animation is done so fire our event
						// TODO - events
						/*
						if( sprite.onAnimationCompletedEvent != null )
							sprite.onAnimationCompletedEvent( sprite.CurrentAnimationKey );
							*/
	
						sprite.IsPlaying = false;
						return;
					}
				}
	
				// time goes backwards when we are reversing a ping-pong loop
				if( sprite.IsLoopingBackOnPingPong )
					elapsedTime = sprite.CurrentAnimation.iterationDuration - elapsedTime;
	
	
				// fetch our desired frame
				var desiredFrame = Mathf.floorToInt( elapsedTime / sprite.CurrentAnimation.secondsPerFrame );
				if( desiredFrame != sprite.CurrentFrame )
				{
					sprite.CurrentFrame = desiredFrame;
					sprite.setSubtexture( sprite.CurrentAnimation.frames[sprite.CurrentFrame] );
					sprite.handleFrameChanged();
	
					// ping-pong needs special care. we don't want to double the frame time when wrapping so we man-handle the totalElapsedTime
					if( sprite.CurrentAnimation.pingPong && ( sprite.CurrentFrame == 0 || sprite.CurrentFrame == sprite.CurrentAnimation.frames.Count - 1 ) )
					{
						if( sprite.IsReversed )
							sprite.TotalElapsedTime -= sprite.CurrentAnimation.secondsPerFrame;
						else
							sprite.TotalElapsedTime += sprite.CurrentAnimation.secondsPerFrame;
					}
				}
		    }
	    }

    }
    
    
}