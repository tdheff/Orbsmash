using Handy.Components;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;
using System.Collections.Generic;

namespace Orbsmash.Player
{
    public enum CharacterChoiceStates { Idle, RotatingLeft, RotatingRight }

    public class CharacterChoiceState : IStateMachineState<CharacterChoiceStates>
    {
        // CONSTANTS
        public const float ROTATION_TIME = 0.5f; // 500 ms
        public const int DistanceFromCenter = 80;
        public const float MOVEMENT_THRESHOLD = 0.1f;
        public List<Gameplay.Character> CharacterOrder = new List<Gameplay.Character>()
        { Gameplay.Character.KNIGHT, Gameplay.Character.WIZARD, Gameplay.Character.SPACEMAN };

        // STATE
        public CharacterChoiceStates StateEnum { get; set; } = CharacterChoiceStates.Idle;
        public int playerId;
        public Gameplay.Character CurrentChoice { get; set; }
        public float LastRotatedTime = 0f;
        
        public CharacterChoiceState(int playerId, Gameplay.Character initialCharacter)
        {
            this.playerId = playerId;
            CurrentChoice = initialCharacter;
        }

        public IStateMachineState<CharacterChoiceStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<CharacterChoiceStates>;
        }
    }
}