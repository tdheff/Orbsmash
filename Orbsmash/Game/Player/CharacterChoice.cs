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

namespace Orbsmash.Player
{
    public class CharacterChoice : Entity
    {
        private int PlayerId;
        private Gameplay.Character InitialCharacter;
        private Vector2 Position;
        // components
        private CharacterChoiceStateMachineComponent _stateComponent;
        private PlayerInputComponent _input;
        public Dictionary<Gameplay.Character, AnimatableSprite> CharacterChoiceSprites = new Dictionary<Gameplay.Character, AnimatableSprite>();
        public Dictionary<Gameplay.Character, AnimationComponent> CharacterChoiceAnimations = new Dictionary<Gameplay.Character, AnimationComponent>();
        public CharacterChoice(int playerId, Gameplay.Character initialCharacter, Vector2 position)
        {
            PlayerId = playerId;
            InitialCharacter = initialCharacter;
            Position = position;
            _input = new PlayerInputComponent(PlayerId);
            _stateComponent = new CharacterChoiceStateMachineComponent(new CharacterChoiceState(PlayerId, InitialCharacter, Position));
            addComponent(_input);
            addComponent(_stateComponent);
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
                        throw new NotImplementedException();
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

        }
    }
}
