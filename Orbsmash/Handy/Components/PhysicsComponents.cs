using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;

namespace Handy.Components
{

    class VelocityComponent : Component
    {
        public Vector2 velocity = new Vector2(0, 0);

        public VelocityComponent() { }

        public VelocityComponent(Vector2 initialVelocity)
        {
            velocity = initialVelocity;
        }
    }
}
