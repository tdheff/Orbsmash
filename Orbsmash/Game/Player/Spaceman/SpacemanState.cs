using Handy.Components;
using Orbsmash.Constants;
using Microsoft.Xna.Framework;
using Optional;

namespace Orbsmash.Player
{
    public enum SpacemanStates { Idle, Walk, Attack, KO, Eliminated, Shield }
    
    public class SpacemanState : IStateMachineState<SpacemanStates>
    {
        // CONSTANTS
        public static readonly float KO_ANIM_PLACEHOLDER_LENGTH = 0.5f;
        public SpacemanStates StateEnum { get; set; } = SpacemanStates.Idle;
        public Vector2 BlockHitVector = new Vector2();
        public float BlockHitTimeRemaining = 0;
        public float KoTime = 0;

        public const float SHIELD_COOLDOWN = 3.0f;
        public float ShieldCooldown = SHIELD_COOLDOWN;
        public Option<Vector2> ShieldSpawn = Option.None<Vector2>();

        public IStateMachineState<SpacemanStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<SpacemanStates>;
        }
    }
}