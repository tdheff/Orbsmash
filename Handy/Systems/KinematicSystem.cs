using System;
using System.Collections.Generic;
using Nez;
using Handy.Components;
using Microsoft.Xna.Framework;

namespace Handy.Systems
{
    public class KinematicSystem : EntitySystem
    {
        public KinematicSystem() : this(new Matcher().all(
                typeof(VelocityComponent),
                typeof(KinematicComponent)
            ).one(typeof(BoxCollider), typeof(CircleCollider)))
        { }

        public KinematicSystem(Matcher matcher) : base(matcher)
        { }

        protected override void process(List<Entity> entities)
        {
            foreach(var entity in entities)
            {
                var velocityComponent = entity.getComponent<VelocityComponent>();
                
                var boxCollider = entity.getComponent<BoxCollider>();
                var circleCollider = entity.getComponent<CircleCollider>();
                Collider collider;
                if (boxCollider != null)
                {
                    collider = boxCollider;
                }
                else
                {
                    collider = circleCollider;
                }

                var kinematicComponent = entity.getComponent<KinematicComponent>();

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
                        entity.transform.position -= collisionResult.minimumTranslationVector;
                        var relativeVelocity = velocityComponent.velocity;
                        if (kinematicComponent.CollisionType == KinematicComponent.ECollisionType.Slide)
                        {
                            calculateSlideResponseVelocity(ref relativeVelocity,
                                ref collisionResult.minimumTranslationVector, out relativeVelocity);
                        }
                        else
                        {
                            calculateBounceResponseVelocity(ref relativeVelocity,
                                ref collisionResult.minimumTranslationVector, out relativeVelocity);
                        }
                        velocityComponent.velocity += relativeVelocity;
                    }
                }
            }
        }

        void calculateSlideResponseVelocity(ref Vector2 relativeVelocity, ref Vector2 minimumTranslationVector, out Vector2 responseVelocity)
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
        
        void calculateBounceResponseVelocity(ref Vector2 relativeVelocity, ref Vector2 minimumTranslationVector, out Vector2 responseVelocity)
        {
            // first, we get the normalized MTV in the opposite direction: the surface normal
            var inverseMTV = minimumTranslationVector * -1f;
            Vector2 normal;
            Vector2.Normalize(ref inverseMTV, out normal);
            
            Console.WriteLine("######");
            Console.WriteLine(relativeVelocity);
            Console.WriteLine(normal);
            Console.WriteLine(relativeVelocity - 2 * Vector2.Dot(relativeVelocity, normal) * normal);

            responseVelocity = (relativeVelocity - 2 * Vector2.Dot(relativeVelocity, normal) * normal) -
                               relativeVelocity;
        }

    }
}
