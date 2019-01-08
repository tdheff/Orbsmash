using System.Diagnostics.Eventing.Reader;
using Nez;

namespace Handy.Components
{
    public class TimerComponent : Component
    {
        public void Set(float duration)
        {
            Duration = duration;
            Elapsed = 0;
        }
        
        public float Duration;
        public float Elapsed = 0;
        public bool Finished
        {
            get => Elapsed >= Duration;
        }
    }
}