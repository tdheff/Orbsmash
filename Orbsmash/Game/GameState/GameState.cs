using Handy.Components;
using System.Collections.Generic;
using System.Linq;
using Orbsmash.Constants;
using Orbsmash.Player;

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
        public GameStates StateEnum { get; set; }

        public bool Served = false;

        public Player.Player[] Players;
        public Ball.Ball Ball;
        public int RightPoints = 0;
        public int LeftPoints = 0;
        
        public IStateMachineState<GameStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<GameStates>;
        }
    }
}