using Handy.Components;

namespace Orbsmash.Game
{
    public class PracticeGameStateComponent : StateMachineComponent<PracticeGameStates, PracticeGameState>
    {
        public PracticeGameStateComponent(PracticeGameState initialState) : base(initialState)
        {
        }
    }
}