using Handy.Components;
using System.Collections.Generic;
using System.Linq;
using Orbsmash.Constants;
using Orbsmash.Player;
using Nez.UI;

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

        // scoreboard obects
        public bool ScoreboardSetup = false;
        public Table Table;
        public Label MainLabel;
        public Label RightScore;
        public Label LeftScore;


        public IStateMachineState<GameStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<GameStates>;
        }
    }
}