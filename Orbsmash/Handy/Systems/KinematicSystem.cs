using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Handy.Components;
using Microsoft.Xna.Framework;

namespace Handy.Systems
{
    class KinematicSystem : EntitySystem
    {
        public static KinematicSystem create()
        {
            return new KinematicSystem(new Matcher().all(
                typeof(VelocityComponent),
                typeof(BoxCollider)
            ));
        }

        public KinematicSystem() : this(new Matcher().all(
                typeof(VelocityComponent),
                typeof(BoxCollider)
            ))
        { }

        public KinematicSystem(Matcher matcher) : base(matcher)
        { }

        protected override void process(List<Entity> entities)
        {
            foreach(var entity in entities)
            {
                var velocityComponent = entity.getComponent<VelocityComponent>();
                var velocity = velocityComponent.velocity;
                var collider = entity.getComponent<BoxCollider>();

                entity.transform.position += velocityComponent.velocity * Time.deltaTime;

                CollisionResult collisionResult;
                // fetch anything that we might collide with at our new position
                var neighbors = Physics.boxcastBroadphaseExcludingSelf(collider, collider.collidesWithLayers);
                foreach (var neighbor in neighbors)
                {
                    // if the neighbor collider is of the same entity, ignore it
                    if (neighbor.entity == entity)
                    {
                        continue;
                    }

                    if (collider.collidesWith(neighbor, out collisionResult))
                    {
                        // neighbor has no ArcadeRigidbody so we assume its immovable and only move ourself
                        entity.transform.position -= collisionResult.minimumTranslationVector;
                        var relativeVelocity = velocity;
                        calculateResponseVelocity(ref relativeVelocity, ref collisionResult.minimumTranslationVector, out relativeVelocity);
                        velocity += relativeVelocity;
                    }
                }
            }
        }

        void calculateResponseVelocity(ref Vector2 relativeVelocity, ref Vector2 minimumTranslationVector, out Vector2 responseVelocity)
        {
            // first, we get the normalized MTV in the opposite direction: the surface normal
            var inverseMTV = minimumTranslationVector * -1f;
            Vector2 normal;
            Vector2.Normalize(ref inverseMTV, out normal);

            // the velocity is decomposed along the normal of the collision and the plane of collision.
            // The elasticity will affect the response along the normal (normalVelocityComponent) and the friction will affect
            // the tangential component of the velocity (tangentialVelocityComponent)
            float n;
            Vector2.Dot(ref relativeVelocity, ref normal, out n);

            var normalVelocityComponent = normal * n;
            var tangentialVelocityComponent = relativeVelocity - normalVelocityComponent;

            if (n > 0.0f)
                normalVelocityComponent = Vector2.Zero;

            // elasticity affects the normal component of the velocity and friction affects the tangential component
            responseVelocity = -(1.0f) * normalVelocityComponent * tangentialVelocityComponent;
        }

    }
}
