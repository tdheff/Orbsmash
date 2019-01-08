using Microsoft.Xna.Framework;
using Nez;

namespace Orbsmash.Player
{
    public class PlayerInputComponent : Component
    {
        public bool DashPressed = false;
        public int DeviceId;

        public Vector2 MovementStick = new Vector2();
        public bool SwingPressed = false;

        public PlayerInputComponent(int deviceId)
        {
            DeviceId = deviceId;
        }
    }
}