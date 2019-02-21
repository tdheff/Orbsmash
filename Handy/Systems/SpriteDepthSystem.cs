using System.Collections.Generic;
using Handy.Components;
using Nez;

namespace Handy.Systems
{
    public class SpriteDepthSystem : EntitySystem
    {
        public SpriteDepthSystem() : base(
            new Matcher().all(typeof(SpriteDepthComponent))
        )
        {
        }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var spriteDepth = entity.getComponent<SpriteDepthComponent>();
                var components = entity.components;
                for (var i = 0; i < components.count; i++)
                {
                    var component = components[i] as RenderableComponent;
                    if (component != null)
                    {
                        component.renderLayer = spriteDepth.BaseRenderLayer - (int)entity.position.Y + component.renderOffset;
                    }
                }
            }
        }
    }
}