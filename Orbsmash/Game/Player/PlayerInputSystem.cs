using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Nez;
using Orbsmash.Animation;

namespace Orbsmash.Player
{
    public class PlayerInputSystem : EntitySystem
    {
        public PlayerInputSystem(Matcher matcher) : base(matcher) { }
        public PlayerInputSystem() : base(new Matcher().all(typeof(PlayerInputComponent))) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var input = entity.getComponent<PlayerInputComponent>();
                // TODO - HACKY TEST
                var sprite = entity.getComponent<SpriteComponent>();
                
                if (Input.gamePads.Length > input.DeviceId)
                {
                    var gamePad = Input.gamePads[input.DeviceId];

                    input.MovementStick = gamePad.getLeftStick();
                    input.MovementStick.Y = -input.MovementStick.Y;
                    input.DashPressed = gamePad.isButtonDown(Buttons.A);
                    input.SwingPressed = gamePad.isButtonDown(Buttons.X);

                    if (input.SwingPressed)
                    {
                        sprite.play(EAnimations.PlayerSwing);
                    }
                }
            }
            
        }
    }
}