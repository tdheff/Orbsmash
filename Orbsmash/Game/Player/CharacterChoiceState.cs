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
        public const float ROTATION_SPEED = 0.1f;
        public const int DistanceFromCenter = 100;
        public List<Gameplay.Character> CharacterOrder = new List<Gameplay.Character>()
        { Gameplay.Character.KNIGHT, Gameplay.Character.WIZARD };

        // STATE
        public CharacterChoiceStates StateEnum { get; set; } = CharacterChoiceStates.Idle;
        public int playerId;
        public Vector2 CenterPosition;
        public Gameplay.Character CurrentChoice;
        
        public CharacterChoiceState(int playerId, Gameplay.Character initialCharacter, Vector2 position)
        {
            this.playerId = playerId;
            CenterPosition = position;
            CurrentChoice = initialCharacter;
        }

        public IStateMachineState<CharacterChoiceStates> Clone()
        {
            return MemberwiseClone() as IStateMachineState<CharacterChoiceStates>;
        }
    }
}