using System.Collections.Generic;
using Handy.Components;
using Nez;

namespace Orbsmash.Game.Effects
{
    public class HitEffectSystem : EntitySystem
    {
        public HitEffectSystem() : base(new Matcher().all(typeof(HitEffectComponent)))
        {
        }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var eventComponent = entity.getComponent<EventComponent>();
                if (eventComponent.ConsumeEventAndReturnIfPresent("END"))
                {
                    entity.destroy();
                }
            }
        }
    }
}