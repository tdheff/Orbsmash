using Handy.Components;
using Orbsmash.Constants;
using Microsoft.Xna.Framework;

namespace Orbsmash.Player
{
    public enum SpacemanStates { Idle, Walk, Attack, KO, Eliminated }
    
    public class SpacemanState : IStateMachineState<SpacemanStates>
    {
        public SpacemanStates StateEnum { get; set; } = SpacemanStates.Idle;
        public Vector2 BlockHitVector = new Vector2();
        public float BlockHitTimeRemaining = 0;

        public IStateMachineState<SpacemanStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<SpacemanStates>;
        }
    }
}