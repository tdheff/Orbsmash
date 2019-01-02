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
        public int PlayerId { get; private set; }
        public Gameplay.Side Side { get; private set; }
        
        // MOVEMENT
        public float Speed = 0;
        public bool CanMove = true;
        public bool DashFinished = false;
        public Vector2 LastVector = new Vector2();
        public Gameplay.Direction LastDirection;

        // SWINGING
        public bool ChargeFinished = false;
        public bool SwingFinished = false;
        
        public PlayerState(int playerId, Gameplay.Side side, float speed)
        {
            PlayerId = playerId;
            Side = side;
            Speed = speed;
        }

        public IStateMachineState<PlayerStates> Clone()
        {
            return new PlayerState(PlayerId, Side, Speed)
            {
                StateEnum = StateEnum,
                CanMove = CanMove
            };
        }
    }
}