using Handy.Components;

namespace Orbsmash.Game
{
    public enum GameStates
    {
        Ready,
        Service,
        Play,
        PointScoredRight,
        PointScoredLeft
    }

    public class GameState : IStateMachineState<GameStates>
    {
        public Ball.Ball Ball;
        public int LeftPoints = 0;

        public Player.Player[] Players;
        public int RightPoints = 0;

        public bool Served = false;
        public GameStates StateEnum { get; set; }

        public IStateMachineState<GameStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<GameStates>;
        }
    }
}