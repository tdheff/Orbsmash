using Handy.Components;

namespace Orbsmash.Player
{
    public class SpacemanStateMachineComponent : StateMachineComponent<SpacemanStates, SpacemanState>
    {
        public SpacemanStateMachineComponent(SpacemanState initialState) : base(initialState)
        {
        }
    }
}