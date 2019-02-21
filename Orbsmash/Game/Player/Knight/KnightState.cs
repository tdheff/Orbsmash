using Handy.Components;
using Orbsmash.Constants;
using Microsoft.Xna.Framework;

namespace Orbsmash.Player
{
    public enum KnightStates { Idle, Walk, Dash, Charge, Swing, KO, Eliminated, Block, BlockHit }
    
    public class KnightState : IStateMachineState<KnightStates>
    {
        public KnightStates StateEnum { get; set; } = KnightStates.Idle;
        public Vector2 BlockHitVector = new Vector2();
        public float BlockHitTimeRemaining = 0;

        public const float MAX_SPRINT = 1.0f;
        public const float MIN_START_SPRINT = 0.9f;
        public const float SPRINT_RECOVERY_MULTIPLIER = 0.3f;
        public float SprintRemaining = MAX_SPRINT;

        public const float BLOCK_COOLDOWN = 6.0f;
        public float BlockCooldown = BLOCK_COOLDOWN;
        
        public IStateMachineState<KnightStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<KnightStates>;
        }
    }
}