using System.Collections.Generic;
using Handy.Animation;
using Handy.Components;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Game.Effects
{
    public class HitEffectComponent : Component {}
    
    public class HitEffect : Entity
    {
        public HitEffect()
        {
            name = "HitEffect";
            scale = new Vector2(2, 2);
        }
        
        public override void onAddedToScene()
        {
            var gameScene = (Handy.Scene)scene;
            var animationDefinition = gameScene.AnimationDefinitions[AsepriteFiles.HIT_EFFECT];

            var sprite = new AnimatableSprite(animationDefinition.SpriteDefinition.Subtextures);
            sprite.renderLayer = RenderLayers.FOREGROUND;
            var animationComponent = new AnimationComponent(sprite, animationDefinition, "DEFAULT");
            var eventComponent = new EventComponent();
            eventComponent.AddEvent("DEFAULT", 3, "END");
            
            addComponent(sprite);
            addComponent(animationComponent);
            addComponent(eventComponent);
            addComponent(new HitEffectComponent());
        }
    }
}