using Handy.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbsmash.Game
{
    public enum PracticeGameStates
    {
        WaitingForPlayersToJoin,
        MaxPlayersJoined,
        AllPlayersOnZones,
    }
    // a lil sub state enum thingy
    public enum PracticeGamePlayerReadyState
    {
        NotJoined,
        Choosing,
        HangingOut,
        ReadyOnLeftSide,
        ReadyOnRightSide
    }
    public class PracticeGameState : IStateMachineState<PracticeGameStates>
    {
        public PracticeGameStates StateEnum { get; set; }
        // we just hide show the chooser or the player depending on what mode they are in
        public Player.Player[] Players;
        public Player.CharacterChoice[] Choosers;
        // by default all 4 players are choosing when they start
        public PracticeGamePlayerReadyState[] PlayerStates = new PracticeGamePlayerReadyState[]
        {
            PracticeGamePlayerReadyState.NotJoined,
            PracticeGamePlayerReadyState.NotJoined,
            PracticeGamePlayerReadyState.NotJoined,
            PracticeGamePlayerReadyState.NotJoined
        };

        public IStateMachineState<PracticeGameStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<PracticeGameStates>;
        }
    }
}
