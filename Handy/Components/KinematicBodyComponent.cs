//using System;
//using Microsoft.Xna.Framework;
//using Nez;
//
//namespace Handy.Components
//{
//    // <summary>
//    // Component for a body that uses simple kinematic physics
//    // </summary>
//    public class KinematicBodyComponent : Component
//    {
//        public enum ECollisionType { Slide, Bounce }
//        
//        # region data
//        
//        public Vector2 Velocity { get; protected set; } = new Vector2();
//        public ECollisionType CollisionType = ECollisionType.Slide;
//        
//        #endregion
//        
//        #region components
//        
//        private CollidableComponent _collider;
//        
//        #endregion
//        
//        public KinematicBodyComponent(bool active) : base(active, false)
//        {
//        }
//        
//        public KinematicBodyComponent(Vector2 initialVelocity) : base(true, false)
//        {
//            Velocity = initialVelocity;
//        }
//        
//        public KinematicBodyComponent() : base(true, false)
//        {
//        }
//
//        public override void Added(Entity entity)
//        {
//            base.Added(entity);
//            _collider = entity.Components.Get<CollidableComponent>();
//        }
//
//        public override void Update()
//        {
//            base.Update();
//
//            Entity.Position += Velocity * Engine.DeltaTime;
//
//            foreach (var collidableComponent in Scene.Tracker.CollidableComponents[typeof(KinematicBodyComponent)])
//            {
//                if (Collide.Check(_collider, collidableComponent))
//                {
//                    Console.WriteLine(collidableComponent.Entity);
//                }
//            }
//        }
//    }
//
//    public class PlayerControlledKinematicBodyComponent : KinematicBodyComponent
//    {
//        public float Speed;
//        private PlayerInputComponent _input;
//
//        public PlayerControlledKinematicBodyComponent(float speed) : base()
//        {
//            Speed = speed;
//        }
//        
//        public override void Added(Entity entity)
//        {
//            base.Added(entity);
//        }
//
//        public override void Update()
//        {
//            Velocity = _input.MovementStick * Speed;
//            base.Update();
//        }
//    }
//}