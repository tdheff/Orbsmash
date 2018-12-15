using Handy.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Orbsmash.Player;

namespace Orbsmash.Ball
{
    public class Ball : Entity
    {
        private readonly VelocityComponent _velocity;
        private readonly CircleCollider _collider;
        private readonly KinematicComponent _kinematic = new KinematicComponent();
        private readonly Sprite<PlayerAnimations> _sprite;
        
        public Ball(Texture2D texture)
        {
            _velocity = new VelocityComponent(new Vector2(100, 100));
            _collider = new CircleCollider();
            _sprite = new Sprite<PlayerAnimations>(new Subtexture(texture, new Rectangle(0, 0, 32, 32), Vector2.Zero));
            
            addComponent(_velocity);
            addComponent(_sprite);
            addComponent(_collider);
            _kinematic.CollisionType = KinematicComponent.ECollisionType.Bounce;
            addComponent(_kinematic);
            
            scale = new Vector2(8, 8);
        }
    }
}