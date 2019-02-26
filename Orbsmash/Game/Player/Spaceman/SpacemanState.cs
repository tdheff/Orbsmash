using Handy.Components;
using Orbsmash.Constants;
using Microsoft.Xna.Framework;

namespace Orbsmash.Player
{
    public enum SpacemanStates { Idle, Walk, Attack, KO, Eliminated }
    
    public class SpacemanState : IStateMachineState<SpacemanStates>
    {
        // CONSTANTS
        public static readonly float KO_ANIM_PLACEHOLDER_LENGTH = 0.5f;
        public SpacemanStates StateEnum { get; set; } = SpacemanStates.Idle;
        public Vector2 BlockHitVector = new Vector2();
        public float BlockHitTimeRemaining = 0;
        public float KoTime = 0;

        public IStateMachineState<SpacemanStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<SpacemanStates>;
        }
    }
}