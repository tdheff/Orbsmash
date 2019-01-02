using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handy.Animation;
using Microsoft.Xna.Framework;
using Nez;

namespace Handy.Components
{
    // <summary>
    // Component for a body that uses simple kinematic physics
    // </summary>
    public class EventComponent : Component
    {
        private float repeatEventFireDelay = 0.1f;
        public HashSet<string> Events = new HashSet<string>();
        private string LastEvent = "";
        private float LastEventTime;
        public EventComponent()
        {
        }

        private bool IsRepeatEvent(string eventName)
        {
            var time = Time.time;
            if(eventName == LastEvent && LastEventTime + repeatEventFireDelay > time)
            {
                return true;
            }
            return false;
        }

        public void FireEvent(string e)
        {
            // this prevents events from just refiring across frames!
            var LastEvent = $"{e}";
            if(!IsRepeatEvent(e))
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
            if(isPresent)
            {
                Events.Remove(e);
            }
            return isPresent;
        }

    }
}
