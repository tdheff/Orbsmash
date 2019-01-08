using Nez;

namespace Handy.Components
{
    // <summary>
    // Component for a body that uses simple kinematic physics
    // </summary>
    public class KinematicComponent : Component
    {
        public enum ECollisionType
        {
            Slide,
            Bounce
        }

        public float BounceDampening = 1.0f;

        public ECollisionType CollisionType = ECollisionType.Slide;
    }
}