using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class PlayerInputComponent : Component
    {
        public const float STICK_THRESHOLD_SQUARED = 0.001f;
        
        public int DeviceId;

        public Vector2 MovementStick
        {
            get
            {
                if (_movementStick.LengthSquared() <= STICK_THRESHOLD_SQUARED)
                {
                    return Vector2.Zero;
                }
                else
                {
                    return _movementStick;
                }
            }
            set
            {
                _movementStick = value;
                if (_movementStick.LengthSquared() >= STICK_THRESHOLD_SQUARED) _movementStick.Normalize();
            }
        }

        private Vector2 _movementStick = Vector2.Zero;
        public bool DashPressed = false;
        public bool AttackPressed = false;
        public bool DefensePressed = false;
        
        public PlayerInputComponent(int deviceId) : base()
        {
            DeviceId = deviceId;
        }
    }
}