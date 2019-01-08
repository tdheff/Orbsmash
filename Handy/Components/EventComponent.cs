using System.Collections.Generic;
using Nez;

namespace Handy.Components
{
    // <summary>
    // Component for a body that uses simple kinematic physics
    // </summary>
    public class EventComponent : Component
    {
        public HashSet<string> Events = new HashSet<string>();
        private readonly string LastEvent = "";
        private float LastEventTime;
        private readonly float repeatEventFireDelay = 0.1f;

        private bool IsRepeatEvent(string eventName)
        {
            var time = Time.time;
            if (eventName == LastEvent && LastEventTime + repeatEventFireDelay > time) return true;
            return false;
        }

        public void FireEvent(string e)
        {
            // this prevents events from just refiring across frames!
            var LastEvent = $"{e}";
            if (!IsRepeatEvent(e))
            {
                Events.Add(e);
                LastEvent = e;
                LastEventTime = Time.time;
            }
        }

        public HashSet<string> PeekEvents()
        {
            return Events;
        }

        public HashSet<string> ConsumeEvents()
        {
            var events = Events;
            Events = new HashSet<string>();
            return events;
        }

        public bool ConsumeEventAndReturnIfPresent(string e)
        {
            var isPresent = Events.Contains(e);
            if (isPresent) Events.Remove(e);
            return isPresent;
        }
    }
}