using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Nez;

namespace Handy.Components
{
    public class HitStopComponent : Component
    {
        public HitStopComponent()
        {
        }
        
        public void Freeze(float duration)
        {
            Duration = duration;
            Remaining = Duration;
            entity.scene.Paused = true;
        }
        
        public float Duration;
        public float Remaining;
    }
}