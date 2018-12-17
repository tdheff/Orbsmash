using Handy.Components;

namespace Orbsmash.Player
{
    public class PlayerStateMachineComponent : StateMachineComponent<PlayerStates, PlayerState>
    {
        public PlayerStateMachineComponent(PlayerState initialState) : base(initialState)
        {
        }
    }
}