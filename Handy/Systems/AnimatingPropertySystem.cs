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
    public class AnimatingPropertySystem : EntitySystem
    {

        public AnimatingPropertySystem()
        {
            _matcher = new Matcher().one(typeof(AnimationComponent));
        }

        protected override void process(List<Entity> entities)
	    {
		    foreach (var entity in entities)
		    {
            }
	    }

    }
    
    
}