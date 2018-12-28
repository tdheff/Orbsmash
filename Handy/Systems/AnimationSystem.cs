using System;
using System.Collections.Generic;
using System.Linq;
using Handy.Animation;
using Handy.Components;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace Handy.Systems
{
    public class AnimationSystem<TEnum> : EntitySystem where TEnum : struct, IComparable, IFormattable
    {
        private IDictionary<string, AnimationDefinition> AnimationDefinitions;

        public AnimationSystem()
        {
            _matcher = new Matcher().one(typeof(AnimationComponent<TEnum>));
        }

        public void SetAnimationDefinitions(IList<AnimationDefinition> animDefs)
        {
            AnimationDefinitions = animDefs.ToDictionary(a => a.Context);
        }
        

        protected override void process(List<Entity> entities)
	    {
		    foreach (var entity in entities)
		    {
                var animations = entity.getComponents<AnimationComponent<TEnum>>();
                foreach(var anim in animations)
                {
                    // this whole thing isn't that clever or efficient in terms of O notation
                    // but i'll worry about that later if it ends up mattering at all
                    // also we'll probably want to add more graceful error handling as we see
                    // how annoying it is to develop when we haven't matched up all the animations and stuff
                    var animationTarget = anim.AnimationTarget;
                    var animationDef = AnimationDefinitions[anim.Context];
                    var currentAnimationTrack = animationDef.Animations.First(x => x.AnimationName == anim.CurrentAnimation.ToString());
                    // let's first elapse the time that has passed
                    var lastElapsedTime = anim.ElapsedTime;
                    anim.ElapsedTime += Time.deltaTime * 1000; // ms
                    // then we find the latest frame... not the most efficient thing in the world but worry bout it later
                    var nextFrame = currentAnimationTrack.Frames.Last(x => x.Time <= anim.ElapsedTime);
                    var animationIsOver = nextFrame.LastFrameLength.HasValue && (nextFrame.LastFrameLength.Value + nextFrame.Time < anim.ElapsedTime);
                    if(animationIsOver)
                    {
                        var overhang = anim.ElapsedTime - nextFrame.LastFrameLength.Value - nextFrame.Time;
                        anim.ElapsedTime = overhang;
                        if (currentAnimationTrack.NextAnimation != null)
                        {
                            Enum.TryParse(currentAnimationTrack.NextAnimation, out TEnum animationEnum);
                            anim.CurrentAnimation = animationEnum;
                            currentAnimationTrack = animationDef.Animations.First(x => x.AnimationName == anim.CurrentAnimation.ToString());
                        }
                        nextFrame = currentAnimationTrack.Frames.Last(x => x.Time <= anim.ElapsedTime);
                    }
                    anim.CurrentFrame = nextFrame.FrameNumber;
                    animationTarget.SetFrame(nextFrame.FrameNumber);
                }



                // TODO - pick the correct "frame" based on the definition + animation . time elapsed
            }
	    }

    }
    
    
}