using Microsoft.Xna.Framework;

namespace Nez.Overlap2D.Runtime
{
    public class PhysicsBodyDataVO
    {
        public bool allowSleep;
        public bool awake;
        public int bodyType = 0;
        public bool bullet;
        public Vector2 centerOfMass;
        public float damping;

        public float density;
        public float friction;
        public float gravityScale;

        public float mass;
        public float restitution;
        public float rotationalInertia;
        public bool sensor;
    }
}