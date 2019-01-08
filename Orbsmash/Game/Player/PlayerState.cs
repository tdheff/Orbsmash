using Handy.Components;
using Microsoft.Xna.Framework;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public enum PlayerStates
    {
        Idle,
        Walk,
        Dash,
        Charge,
        Swing,
        Dead
    }

    public class PlayerState : IStateMachineState<PlayerStates>
    {
        // CONSTANTS
        public const float MovementThresholdSquared = 0.01f;
        public const float MaxChargeTime = 2.0f;
        public bool CanMove = true;
        public bool ChargeFinished = false;

        // SWINGING
        public float ChargeTime;
        public bool DashFinished = false;

        // OTHER
        public bool IsInvulnerable = false;
        public bool IsKilled = false;
        public Gameplay.Direction LastDirection;
        public Vector2 LastVector = new Vector2();
        public Vector2 ResetPosition;

        // MOVEMENT
        public float Speed;
        public bool SwingFinished = false;

        public PlayerState(int playerId, Gameplay.Side side, float speed, Vector2 resetPosition)
        {
            this.playerId = playerId;
            this.side = side;
            Speed = speed;
            ResetPosition = resetPosition;
        }

        // IDENTIFICATION
        public int playerId { get; }
        public Gameplay.Side side { get; }

        // STATE
        public PlayerStates StateEnum { get; set; } = PlayerStates.Idle;

        public IStateMachineState<PlayerStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<PlayerStates>;
        }
    }
}