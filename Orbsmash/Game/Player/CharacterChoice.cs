using Handy.Animation;
using Microsoft.Xna.Framework;
using HandyScene = Handy.Scene;
using Nez;
using Orbsmash.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handy.Components;
using Handy;

namespace Orbsmash.Player
{
    public class CharacterChoice : Entity
    {
        // constants
        private int Radius = 100;

        private int PlayerId;
        private Gameplay.Character InitialCharacter;
        // components
        public CharacterChoiceStateMachineComponent _stateComponent;
        private PlayerInputComponent _input;
        public Dictionary<Gameplay.Character, AnimatableSprite> CharacterChoiceSprites = new Dictionary<Gameplay.Character, AnimatableSprite>();
        public Dictionary<Gameplay.Character, AnimationComponent> CharacterChoiceAnimations = new Dictionary<Gameplay.Character, AnimationComponent>();
        public AnimatingPropertyComponent RotationAngle = new AnimatingPropertyComponent();

        public CharacterChoice(int playerId, Gameplay.Character initialCharacter, Vector2 position)
        {
            PlayerId = playerId;
            InitialCharacter = initialCharacter;
            this.position = position;
            _input = new PlayerInputComponent(PlayerId);
            _stateComponent = new CharacterChoiceStateMachineComponent(new CharacterChoiceState(PlayerId, InitialCharacter));
            addComponent(RotationAngle);
            addComponent(_input);
            addComponent(_stateComponent);
            SetRotationAngleFixedAtCurrentChoice();
        }

        public void SetRotationAngleFixedAtCurrentChoice()
        {
            // 0 degrees = first choice
            var state = _stateComponent.State;
            var numChoices = state.CharacterOrder.Count;
            var positionOfCurrentChoice = state.CharacterOrder.FindIndex(c => c == state.CurrentChoice);
            var angle = (360f / numChoices) * positionOfCurrentChoice;
            RotationAngle.SetParameters(angle, angle, 1, AnimationSchemes.CircleDegrees, false); // just sort of hold it there
        }

        public void SetRotationAngleToMoveTowardsCurrentChoice()
        {
            // 0 degrees = first choice
            var currentAngle = RotationAngle.GetCurrentValue();
            var state = _stateComponent.State;
            var numChoices = state.CharacterOrder.Count;
            var positionOfNewChoice = state.CharacterOrder.FindIndex(c => c == state.CurrentChoice);
            var newAngle = (360f / numChoices) * positionOfNewChoice;
            Console.WriteLine($"rotating towards current choice. CurrentAngle: {currentAngle}; New Angle: {newAngle}");
            RotationAngle.SetParameters(currentAngle, newAngle, CharacterChoiceState.ROTATION_TIME, AnimationSchemes.CircleDegrees, false);
            RotationAngle.Start(); // kick it off!
        }


        public void SetSpritePositionsFromCurrentState()
        {
            var state = _stateComponent.State;
            var numChoices = state.CharacterOrder.Count;
            var gapBetween = 360f / numChoices;
            var positionOfCurrentChoice = state.CharacterOrder.FindIndex(c => c == state.CurrentChoice);
            for (var i = 0; i < numChoices; i++)
            {
                var character = state.CharacterOrder[i];
                // as if 0 = first choice, 90 deg = 1/4 choices, 180 = 2/4, etc that kinda thing
                var angle = (360f / numChoices) * i;
                // but instead we are rotated additionally by rotationAngle
                angle = angle - RotationAngle.GetCurrentValue();
                // now subtract 90 cuz the bottom is "selected"
                angle = angle - 90;
                var vector = HandyMath.MakeNormalizedVectorFromAngleDegrees(angle);
                vector = vector * CharacterChoiceState.DistanceFromCenter;
                var sprite = CharacterChoiceSprites[character];
                sprite.setLocalOffset(new Vector2(vector.X, -1 * vector.Y)); // cuz in this world Y is negative...
            }
        }

        public override void onAddedToScene()
        {
            var gameScene = (HandyScene)scene;

            foreach (var character in _stateComponent.State.CharacterOrder)
            {
                // not moving this to shared code
                // as we may decide we want different sprites/animations for the choice menu
                // e.g. "them posing and looking dope" or whatever
                AnimationDefinition animationDefinition;
                AnimatableSprite sprite;
                switch (character)
                {
                    case Gameplay.Character.KNIGHT:
                        animationDefinition = gameScene.AnimationDefinitions[AsepriteFiles.KNIGHT];
                        sprite = new AnimatableSprite(animationDefinition.SpriteDefinition.Subtextures);
                        CharacterChoiceSprites.Add(character, sprite);
                        CharacterChoiceAnimations.Add(character, new AnimationComponent(sprite, animationDefinition, KnightAnimations.WALK_FORWARD));
                        break;
                    case Gameplay.Character.WIZARD:
                        animationDefinition = gameScene.AnimationDefinitions[AsepriteFiles.WIZARD];
                        sprite = new AnimatableSprite(animationDefinition.SpriteDefinition.Subtextures);
                        CharacterChoiceSprites.Add(character, sprite);
                        CharacterChoiceAnimations.Add(character, new AnimationComponent(sprite, animationDefinition, WizardAnimations.WALK_DOWN));
                        break;
                    case Gameplay.Character.SPACEMAN:
                        animationDefinition = gameScene.AnimationDefinitions[AsepriteFiles.SPACEMAN];
                        sprite = new AnimatableSprite(animationDefinition.SpriteDefinition.Subtextures);
                        CharacterChoiceSprites.Add(character, sprite);
                        CharacterChoiceAnimations.Add(character, new AnimationComponent(sprite, animationDefinition, SpacemanAnimations.WALK_DOWN));
                        break;
                    case Gameplay.Character.ALIEN:
                        throw new NotImplementedException();
                    case Gameplay.Character.PIRATE:
                        throw new NotImplementedException();
                    case Gameplay.Character.SKELETON:
                        throw new NotImplementedException();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            CharacterChoiceSprites.Values.ToList().ForEach(s => addComponent(s));
            CharacterChoiceSprites.Values.ToList().ForEach(s => s.renderLayer = RenderLayers.PRIMARY);
            CharacterChoiceAnimations.Values.ToList().ForEach(a => addComponent(a));
        }
    }
}
