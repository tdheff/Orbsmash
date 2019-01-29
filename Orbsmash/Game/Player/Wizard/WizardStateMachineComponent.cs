using System.Security.Cryptography.X509Certificates;
using Handy.Components;

namespace Orbsmash.Player
{
    public class WizardStateMachineComponent : StateMachineComponent<WizardStates, WizardState>
    { 
        public WizardStateMachineComponent(WizardState initialState) : base(initialState)
        {
        }
    }
}