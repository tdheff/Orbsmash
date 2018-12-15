using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace Orbsmash.Player
{
    public class PlayerInputComponent : Component
    {
        public int DeviceId;
        
        public Vector2 MovementStick = new Vector2();
        public bool DashPressed = false;
        public bool SwingPressed = false;
        
        public PlayerInputComponent(int deviceId) : base()
        {
            DeviceId = deviceId;
        }
    }
}