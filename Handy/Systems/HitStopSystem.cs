using System.Collections.Generic;
using Handy.Components;
using Nez;

namespace Handy.Systems
{
    public class HitStopSystem : EntitySystem
    {
        public HitStopSystem() : base(new Matcher().all(typeof(HitStopComponent)))
        {
            Pausable = false;
        }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var hitstop = entity.getComponent<HitStopComponent>();
                if (hitstop.Remaining <= 0)
                {
                    Time.timeScale = 1.0f;
                }

                hitstop.Remaining -= Time.altDeltaTime;
            }
        }
    }
}