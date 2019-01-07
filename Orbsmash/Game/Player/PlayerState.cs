using Handy.Components;
using Orbsmash.Constants;
using Microsoft.Xna.Framework;

namespace Orbsmash.Player
{
    public enum PlayerStates { Idle, Walk, Dash, Charge, Swing, Dead }
    
    public class PlayerState : IStateMachineState<PlayerStates>
    {
        // CONSTANTS
        public const float MovementThresholdSquared = 0.01f;
        
        // STATE
        public PlayerStates StateEnum { get; set; } = PlayerStates.Idle;
        public bool IsKilled = false;
        
        // IDENTIFICATION
        public int playerId { get; private set; }
        public Gameplay.Side side { get; private set; }
        
        // MOVEMENT
        public float Speed;
        public bool CanMove = true;
        public bool DashFinished = false;
        public Vector2 LastVector = new Vector2();
        public Gameplay.Direction LastDirection;
        public Vector2 ResetPosition;

        // SWINGING
        public float ChargeTime;
        public const float MaxChargeTime = 2.0f;
        public bool ChargeFinished = false;
        public bool SwingFinished = false;
        
        // OTHER
        public bool IsInvulnerable = false;
        
        public PlayerState(int playerId, Gameplay.Side side, float speed, Vector2 resetPosition)
        {
            this.playerId = playerId;
            this.side = side;
            Speed = speed;
            ResetPosition = resetPosition;
        }

        public IStateMachineState<PlayerStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<PlayerStates>;
        }
    }
}