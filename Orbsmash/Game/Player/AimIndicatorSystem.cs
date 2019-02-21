using System;
using System.Collections.Generic;
using Handy.Components;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class AimIndicatorSystem : EntitySystem
    {
        public AimIndicatorSystem() : base(new Matcher().all(typeof(AimIndicatorComponent))) {}

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var aimIndicatorComponent = entity.getComponent<AimIndicatorComponent>();
                var input = aimIndicatorComponent.player.getComponent<PlayerInputComponent>();
                var state = aimIndicatorComponent.player.getComponent<PlayerStateComponent>();
                var hitVector = Player.CalculateHitVector(state.side, input.MovementStick);

                entity.transform.scale = aimIndicatorComponent.player.transform.scale;
                entity.transform.position = aimIndicatorComponent.player.transform.position;
                entity.transform.localRotation = Mathf.atan2(hitVector.Y, hitVector.X);
                
                var sprite = entity.getComponent<SpriteComponent>();
                sprite.localOffset = new Vector2(0, 0);
                    // state.side == Gameplay.Side.LEFT ? new Vector2(14 * 5, 0) : new Vector2(-14 * 5, 0);
            }
        }
    }
}