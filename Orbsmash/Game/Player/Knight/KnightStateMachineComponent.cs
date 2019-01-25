using Handy.Components;

namespace Orbsmash.Player
{
    public class KnightStateMachineComponent : StateMachineComponent<KnightStates, KnightState>
    {
        public KnightStateMachineComponent(KnightState initialState) : base(initialState)
        {
        }
    }
}