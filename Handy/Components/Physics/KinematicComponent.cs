using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;
using Optional;

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
        };

        public ECollisionType CollisionType = ECollisionType.Slide;
        public float BounceDampening = 1.0f;
        public Collider LastCollision;
    }
}
