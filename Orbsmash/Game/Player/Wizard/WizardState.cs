using Handy.Components;
using Orbsmash.Constants;
using Microsoft.Xna.Framework;

namespace Orbsmash.Player
{
    public enum WizardStates { Idle, Walk, Attack, Dead }
    
    public class WizardState : IStateMachineState<WizardStates>
    {
        // STATE
        public WizardStates StateEnum { get; set; } = WizardStates.Idle;

        public IStateMachineState<WizardStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<WizardStates>;
        }
    }
}