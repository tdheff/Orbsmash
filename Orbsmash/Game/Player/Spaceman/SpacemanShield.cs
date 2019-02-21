using System.Collections.Generic;
using Handy.Animation;
using Handy.Components;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;
using HandyScene = Handy.Scene;

namespace Orbsmash.Player
{
    public class SpacemanShieldComponent : Component
    {
        public Gameplay.Side Side;
    }

    public class SpacemanShield : Entity
    {
        private static Dictionary<string, HashSet<string>> _triggers = new Dictionary<string, HashSet<string>>
        {
            { EventComponent.BuildKey(SpacemanAnimations.SHIELD, 35 ), new HashSet<string> { PlayerEvents.BLOCK_END }},
        };

        public SpacemanShield(Gameplay.Side side)
        {
            addComponent(new SpacemanShieldComponent { Side = side});
            addComponent(new SpriteDepthComponent { BaseRenderLayer = RenderLayers.PRIMARY});
        }
        
        public override void onAddedToScene()
        {
            // TODO - this should happen automatically
            scale = new Vector2(5, 5);
            
            var gameScene = (HandyScene)scene;
            var animationDefinition = gameScene.AnimationDefinitions[AsepriteFiles.SPACEMAN_SHIELD];

            var sprite = new AnimatableSprite(animationDefinition.SpriteDefinition.Subtextures);
            sprite.renderLayer = RenderLayers.PRIMARY;
            
            var animation = new AnimationComponent(sprite, animationDefinition, SpacemanAnimations.SHIELD);
            
            var collider = new BoxCollider(5*1.5f, 5*5.5f);
            collider.localOffset = new Vector2(5, 0);
            Flags.setFlagExclusive(ref collider.physicsLayer, PhysicsLayers.BACK_WALLS);
            Flags.setFlag(ref collider.collidesWithLayers, PhysicsLayers.BALL);
            
            var events = new EventComponent();
            events.SetTriggers(_triggers);
            
            addComponent(sprite);
            addComponent(animation);
            addComponent(events);
            addComponent(collider);
            
            position = new Vector2(position.X + 5*15, position.Y);
        }
    }
}