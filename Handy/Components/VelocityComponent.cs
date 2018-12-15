using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Nez;

namespace Handy.Components
{
    public class VelocityComponent : Component
    {
        public Vector2 Velocity = new Vector2(0, 0);

        public VelocityComponent() { }

        public VelocityComponent(Vector2 initialVelocity)
        {
            Velocity = initialVelocity;
        }
    }
}