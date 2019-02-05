using Nez;

namespace Handy.Components
{
    public class CameraShakeComponent : Component
    {
        public float Duration = 1.0f;
        public float Intensity = 1.0f;

        public float RemainingDuration = 0.0f;

        public void Shake(float duration = 1.0f, float intensity = 1.0f)
        {
            Duration = duration;
            Intensity = intensity;
            RemainingDuration = Duration;
        }
    }
}