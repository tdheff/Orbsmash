using Handy.Components;

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
        public int PlayerId { get; private set; }
        public Constants.Side Side { get; private set; }
        
        // MOVEMENT
        public float Speed = 300.0f;
        public bool CanMove = true;
        public bool DashFinished = false;
        
        // SWINGING
        public bool ChargeFinished = false;
        public bool SwingFinished = false;
        
        public PlayerState(int playerId, Constants.Side side)
        {
            PlayerId = playerId;
            Side = side;
        }

        public IStateMachineState<PlayerStates> Clone()
        {
            return new PlayerState(PlayerId, Side)
            {
                StateEnum = StateEnum,
                Speed = Speed,
                CanMove = CanMove
            };
        }
    }
}