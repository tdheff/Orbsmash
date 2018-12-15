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

//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using Monocle;
//
//namespace Orbsmash.Player
//{
//    public class PlayerInputComponent : Component
//    {
//        public int DeviceId;
//        
//        public Vector2 MovementStick { get; private set; } = new Vector2();
//        public bool DashPressed { get; private set; } = false;
//        public bool SwingPressed { get; private set; } = false;
//        
//        public PlayerInputComponent(bool active, bool visible, int deviceId) : base(active, visible)
//        {
//            DeviceId = deviceId;
//        }
//
//        public override void Update()
//        {
//            base.Update();
//
//            if (MInput.GamePads.Length > DeviceId)
//            {
//                MInput.GamePadData gamePad = MInput.GamePads[DeviceId];
//
//                MovementStick = gamePad.GetLeftStick();
//                DashPressed = gamePad.Pressed(Buttons.A);
//                SwingPressed = gamePad.Pressed(Buttons.X);
//            }
//            
//        }
//    }
//}