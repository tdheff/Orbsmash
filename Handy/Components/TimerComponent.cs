using Nez;

namespace Handy.Components
{
    public class TimerComponent : Component
    {
        public float Duration;
        public float Elapsed;

        public bool Finished => Elapsed >= Duration;

        public void Set(float duration)
        {
            Duration = duration;
            Elapsed = 0;
        }
    }
}