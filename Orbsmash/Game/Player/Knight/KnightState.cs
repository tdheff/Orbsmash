using Handy.Components;
using Orbsmash.Constants;
using Microsoft.Xna.Framework;

namespace Orbsmash.Player
{
    public enum KnightStates { Idle, Walk, Dash, Charge, Swing, Dead, Block }
    
    public class KnightState : IStateMachineState<KnightStates>
    {
        // STATE
        public KnightStates StateEnum { get; set; } = KnightStates.Idle;

        public IStateMachineState<KnightStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<KnightStates>;
        }
    }
}