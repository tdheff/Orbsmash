using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Handy.Animation;
using Handy.Components;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace Handy.Systems
{
    public class AnimationSystem : EntitySystem
    {
        private IDictionary<string, AnimationDefinition> AnimationDefinitions;

        public AnimationSystem()
        {
            _matcher = new Matcher().one(typeof(AnimationComponent));
        }

        protected override void process(List<Entity> entities)
	    {
		    foreach (var entity in entities)
		    {
                var animations = entity.getComponents<AnimationComponent>();
                var eventComp = entity.getComponent<EventComponent>();
                foreach(var anim in animations)
                {
                    if (anim.Paused) continue;
                    
                    var animationTarget = anim.AnimationTarget;
                    var animationDef = anim.AnimationDefinition;
                    if (animationDef == null)
                    {
                        animationTarget.SetFrame(0);
                        return;
                    }
                    var currentAnimationTrack = animationDef.Animations.First(x => x.AnimationName == anim.CurrentAnimation.ToString());
                    // let's first elapse the time that has passed
                    anim.CurrentAnimationElapsedTime += Time.deltaTime * 1000 * anim.PlaybackSpeed; // ms
                    anim.CurrentFrameElapsedTime += Time.deltaTime * 1000 * anim.PlaybackSpeed; // ms

                    if (anim.CurrentFrameElapsedTime >= currentAnimationTrack.Frames[anim.CurrentAnimationFrame].Duration)
                    {
                        anim.CurrentAnimationFrame = (anim.CurrentAnimationFrame + 1) % currentAnimationTrack.Frames.Count;
                        anim.CurrentFrameElapsedTime = 0;
                        
                        animationTarget.SetFrame(currentAnimationTrack.Frames[anim.CurrentAnimationFrame].FrameNumber);
                    }

                    // ###### EVENT SECTION #######
                    // gonna use the "old" animation track to handle events that are like.. finishing and stuff
                    // also need the "old" elapsed time to catch things at the end of animations
                    if(eventComp != null)
                    {
                        var eventTriggers = eventComp.GetEventTriggers(currentAnimationTrack.AnimationName,
                            anim.CurrentAnimationFrame);
                        foreach (var ev in eventTriggers)
                        {
                            eventComp.FireEvent(ev);
                        }
                    }

                }



                // TODO - pick the correct "frame" based on the definition + animation . time elapsed
            }
	    }

    }
    
    
}