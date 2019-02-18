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

        public IStateMachineState<KnightStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<KnightStates>;
        }
    }
}