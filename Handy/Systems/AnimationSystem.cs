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
            // TODO - make this not sprite name but "track name" or some shit
            AnimationDefinitions = animDefs.ToDictionary(a => a.SpriteName);
        }
        

        protected override void process(List<Entity> entities)
	    {
		    foreach (var entity in entities)
		    {

                var animation = entity.getComponent<AnimationComponent<TEnum>>();
                var animationTarget = animation.AnimationTarget;
                var animationDef = AnimationDefinitions[animation.AnimationTrackIdentifier];

                // TODO - pick the correct "frame" based on the definition + animation . time elapsed
		    }
	    }

    }
    
    
}