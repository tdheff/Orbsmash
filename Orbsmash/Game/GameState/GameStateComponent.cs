using Handy.Components;

namespace Orbsmash.Game
{
    public class GameStateComponent : StateMachineComponent<GameStates, GameState>
    {
        public GameStateComponent(GameState initialState) : base(initialState)
        {
        }
    }
}