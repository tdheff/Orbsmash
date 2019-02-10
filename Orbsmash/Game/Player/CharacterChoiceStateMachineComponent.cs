using System.Security.Cryptography.X509Certificates;
using Handy.Components;

namespace Orbsmash.Player
{
    public class CharacterChoiceStateMachineComponent : StateMachineComponent<CharacterChoiceStates, CharacterChoiceState>
    { 
        public CharacterChoiceStateMachineComponent(CharacterChoiceState initialState) : base(initialState)
        {
        }
    }
}