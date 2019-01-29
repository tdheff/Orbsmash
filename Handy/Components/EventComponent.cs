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
        private Dictionary<string, HashSet<string>> _eventTriggers = new Dictionary<string, HashSet<string>>();
        public HashSet<string> Events = new HashSet<string>();
        private string _lastEvent = "";
        private float _lastEventTime;
        public EventComponent()
        {
        }
        
        public EventComponent(Dictionary<string, HashSet<string>> eventTriggers)
        {
            _eventTriggers = eventTriggers;
        }

        public void SetTriggers(Dictionary<string, HashSet<string>> eventTriggers)
        {
            _eventTriggers = eventTriggers;
        }

        public static string BuildKey(string animationName, int frame)
        {
            return $"{animationName}##{frame}";
        }

        public HashSet<string> GetEventTriggers(string animationName, int frame)
        {
            if (!_eventTriggers.ContainsKey(BuildKey(animationName, frame)))
            {
                return new HashSet<string>();
            }

            return _eventTriggers[BuildKey(animationName, frame)];
        }
        
        public void AddEvent(string animationName, int frame, string eventName)
        {
            if (!_eventTriggers.ContainsKey(BuildKey(animationName, frame)))
            {
                _eventTriggers[BuildKey(animationName, frame)] = new HashSet<string> { eventName };
            }
            else
            {
                _eventTriggers[BuildKey(animationName, frame)].Add(eventName);
            }
        }

        public void DeleteEvent(string animationName, int frame, string eventName)
        {
            if (!_eventTriggers.ContainsKey(BuildKey(animationName, frame)))
            {
                return;
            }

            _eventTriggers[BuildKey(animationName, frame)].Remove(eventName);
        }

        private bool IsRepeatEvent(string eventName)
        {
            var time = Time.time;
            if(eventName == _lastEvent && _lastEventTime + repeatEventFireDelay > time)
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
                _lastEventTime = Time.time;
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
