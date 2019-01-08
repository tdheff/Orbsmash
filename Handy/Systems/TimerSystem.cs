using System.Collections.Generic;
using Handy.Components;
using Nez;

namespace Handy.Systems
{
    public class TimerSystem : EntitySystem
    {
        public TimerSystem() : base(new Matcher().all(typeof(TimerComponent)))
        {
        }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var timers = entity.getComponents<TimerComponent>();
                foreach (var timer in timers) timer.Elapsed += Time.deltaTime;
            }
        }
    }
}