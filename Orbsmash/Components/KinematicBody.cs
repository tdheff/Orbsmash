using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;

namespace Orbsmash.Components
{
    class KinematicBody : Component, IUpdatable
    {
        // The velocity of the kinematic body
        public Vector2 velocity;

        // The collider for handling collisions
        public Collider collider;

        // use gravity
        public bool useGravity; 

        public void update()
        {
            if (useGravity)
            {
                velocity += Physics.gravity * Time.deltaTime;
            }

            entity.transform.position += velocity * Time.deltaTime;
        }
    }
}
