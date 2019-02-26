using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

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

                // ditch if we don't have enough inputs
                if (Input.gamePads.Length <= input.DeviceId) continue;
                
                var gamePad = Input.gamePads[input.DeviceId];
                input.MovementStick = gamePad.getLeftStick();
                Console.WriteLine(input.MovementStick.Y);
                input.MovementStick = new Vector2(input.MovementStick.X, -input.MovementStick.Y);
                if (Math.Abs(input.MovementStick.X) < PlayerStateComponent.DEAD_ZONE)
                {
                    input.MovementStick = new Vector2(0, input.MovementStick.Y);
                }
                if (Math.Abs(input.MovementStick.Y) < PlayerStateComponent.DEAD_ZONE)
                {
                    input.MovementStick = new Vector2(input.MovementStick.X, 0);
                }
                input.DashPressed = gamePad.isButtonDown(Buttons.A);
                input.AttackPressed = gamePad.isButtonDown(Buttons.X) || gamePad.isRightTriggerDown() || gamePad.isButtonDown(Buttons.RightShoulder);
                input.HeavyAttackPressed = gamePad.isButtonDown(Buttons.B)|| gamePad.isLeftTriggerDown() || gamePad.isButtonDown(Buttons.LeftShoulder);
            }
            
        }
    }
}