using System.Collections.Generic;
using System.Linq;
using Handy.Animation;
using Handy.Components;
using Nez;

namespace Handy.Systems
{
    public class AnimationSystem : EntitySystem
    {
        private IDictionary<string, AnimationDefinition> AnimationDefinitions;

        public AnimationSystem()
        {
            _matcher = new Matcher().one(typeof(AnimationComponent));
        }

        public void SetAnimationDefinitions(IList<AnimationDefinition> animDefs)
        {
            AnimationDefinitions = animDefs.ToDictionary(a => a.Context);
        }


        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var animations = entity.getComponents<AnimationComponent>();
                var eventComp = entity.getComponent<EventComponent>();
                foreach (var anim in animations)
                {
                    // ###### SPRITE SECTION #######
                    // this whole thing isn't that clever or efficient in terms of O notation
                    // but i'll worry about that later if it ends up mattering at all
                    // also we'll probably want to add more graceful error handling as we see
                    // how annoying it is to develop when we haven't matched up all the animations and stuff
                    var animationTarget = anim.AnimationTarget;
                    var animationDef = AnimationDefinitions[anim.Context];
                    var currentAnimationTrack =
                        animationDef.Animations.First(x => x.AnimationName == anim.CurrentAnimation.ToString());
                    // let's first elapse the time that has passed
                    anim.ElapsedTime += Time.deltaTime * 1000; // ms
                    var storedElapsedTime = anim.ElapsedTime;
                    // then we find the latest frame... not the most efficient thing in the world but worry bout it later
                    var nextFrame = currentAnimationTrack.Frames.Last(x => x.Time <= anim.ElapsedTime);
                    var animationIsOver = nextFrame.LastFrameLength.HasValue &&
                                          nextFrame.LastFrameLength.Value + nextFrame.Time < anim.ElapsedTime;
                    if (animationIsOver)
                    {
                        var overhang = anim.ElapsedTime - nextFrame.LastFrameLength.Value - nextFrame.Time;
                        anim.ElapsedTime = overhang;
//                        Console.WriteLine($"###### Animation over: {anim.CurrentAnimation} and next is: {currentAnimationTrack.NextAnimation} ######");
                        if (currentAnimationTrack.NextAnimation != null)
                        {
                            anim.CurrentAnimation = currentAnimationTrack.NextAnimation;
                            currentAnimationTrack = animationDef.Animations.First(x =>
                                x.AnimationName == anim.CurrentAnimation.ToString());
                        }

                        nextFrame = currentAnimationTrack.Frames.Last(x => x.Time <= anim.ElapsedTime);
                    }

                    anim.CurrentFrame = nextFrame.FrameNumber;
                    animationTarget.SetFrame(nextFrame.FrameNumber);

                    // ###### EVENT SECTION #######
                    // gonna use the "old" animation track to handle events that are like.. finishing and stuff
                    // also need the "old" elapsed time to catch things at the end of animations
                    if (eventComp != null)
                        if (currentAnimationTrack.Events != null && currentAnimationTrack.Events.Count > 0)
                        {
                            var latestEvent =
                                currentAnimationTrack.Events.LastOrDefault(x => x.Time <= storedElapsedTime);
                            if (latestEvent != null) eventComp.FireEvent(latestEvent.EventName);
                        }
                }


                // TODO - pick the correct "frame" based on the definition + animation . time elapsed
            }
        }
    }
}