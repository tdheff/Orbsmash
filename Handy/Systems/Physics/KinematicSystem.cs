using System;
using System.Collections.Generic;
using System.Linq;
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
            ).one(typeof(BoxCollider), typeof(CircleCollider), typeof(PolygonCollider)))
        { }

        public KinematicSystem(Matcher matcher) : base(matcher)
        { }

        protected override void process(List<Entity> entities)
        {
            foreach(var entity in entities)
            {
                var velocityComponent = entity.getComponent<VelocityComponent>();
                
                var boxColliders = entity.getComponents<BoxCollider>();
                var circleColliders = entity.getComponents<CircleCollider>();
                var polygonColliders = entity.getComponents<PolygonCollider>();
                
                Collider collider = null;
                foreach (var colliderComponent in new List<Collider>().Concat(boxColliders).Concat(circleColliders).Concat(polygonColliders))
                {
                    if (!colliderComponent.isTrigger)
                    {
                        if (collider != null)
                        {
                            Debug.warn("Multiple suitable colliders found for this kinematic body, using: {}", collider);
                        }
                        else
                        {
                            collider = colliderComponent;
                        }
                    }
                }

                var kinematicComponent = entity.getComponent<KinematicComponent>();

                float initialVelocityLength = velocityComponent.Velocity.Length();

                entity.transform.position += velocityComponent.Velocity * Time.deltaTime;

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

                    if (neighbor.isTrigger)
                    {
                        
                        continue;
                    }

                    if (collider.collidesWith(neighbor, out collisionResult))
                    {
                        entity.transform.position -= collisionResult.minimumTranslationVector;
                        var relativeVelocity = velocityComponent.Velocity;
                        if (kinematicComponent.CollisionType == KinematicComponent.ECollisionType.Slide)
                        {
                            calculateSlideResponseVelocity(ref relativeVelocity,
                                ref collisionResult.minimumTranslationVector, out relativeVelocity);
                            velocityComponent.Velocity += relativeVelocity;

                        }
                        else
                        {
                            calculateBounceResponseVelocity(ref relativeVelocity,
                                ref collisionResult.minimumTranslationVector, out relativeVelocity);
                            velocityComponent.Velocity += relativeVelocity;
                        }

                        kinematicComponent.LastCollision = neighbor;
                    }
                }

                if (velocityComponent.Velocity.Length() > initialVelocityLength)
                {
                    velocityComponent.Velocity = Vector2.Normalize(velocityComponent.Velocity) * initialVelocityLength;
                }
            }
        }

        void calculateSlideResponseVelocity(ref Vector2 relativeVelocity, ref Vector2 minimumTranslationVector, out Vector2 responseVelocity)
        {
            if (minimumTranslationVector.LengthSquared() < float.Epsilon)
            {
                responseVelocity = relativeVelocity;
                return;
            }
            // first, we get the normalized MTV in the opposite direction: the surface normal
            var inverseMtv = minimumTranslationVector * -1f;
            Vector2.Normalize(ref inverseMtv, out var normal);

            // the velocity is decomposed along the normal of the collision and the plane of collision.
            // The elasticity will affect the response along the normal (normalVelocityComponent) and the friction will affect
            // the tangential component of the velocity (tangentialVelocityComponent)
            Vector2.Dot(ref relativeVelocity, ref normal, out var n);

            var normalVelocityComponent = normal * n;
            var tangentialVelocityComponent = relativeVelocity - normalVelocityComponent;

            if (n > 0.0f)
                normalVelocityComponent = Vector2.Zero;

            // elasticity affects the normal component of the velocity and friction affects the tangential component
            responseVelocity = -(1.0f) * normalVelocityComponent * tangentialVelocityComponent;
        }
        
        void calculateBounceResponseVelocity(ref Vector2 relativeVelocity, ref Vector2 minimumTranslationVector, out Vector2 responseVelocity)
        {
            if (minimumTranslationVector.LengthSquared() < float.Epsilon)
            {
                responseVelocity = relativeVelocity;
                return;
            }
            // first, we get the normalized MTV in the opposite direction: the surface normal
            var inverseMtv = minimumTranslationVector * -1f;
            Vector2.Normalize(ref inverseMtv, out var normal);

            var reflectedVector = (relativeVelocity - 2 * Vector2.Dot(relativeVelocity, normal) * normal);
            responseVelocity = reflectedVector - relativeVelocity;
        }

    }
}
