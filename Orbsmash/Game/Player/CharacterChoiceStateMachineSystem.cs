using System;
using System.Linq;
using Handy;
using Handy.Components;
using Handy.Systems;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class CharacterChoiceStateMachineSystem : PushdownAutomatonSystem<CharacterChoiceStates, CharacterChoiceState, CharacterChoiceStateMachineComponent>
    {
        public CharacterChoiceStateMachineSystem() : base(new Matcher().all(
            typeof(CharacterChoiceStateMachineComponent),
            typeof(PlayerInputComponent),
            typeof(EventComponent)
        )) { }

        protected override void Update(Entity entity, CharacterChoiceStateMachineComponent stateMachine)
        {
            
        }

        protected override StateMachineTransition<CharacterChoiceStates> Transition(Entity entity, CharacterChoiceStateMachineComponent stateMachine)
        {
            return StateMachineTransition<CharacterChoiceStates>.None();
        }

        protected override void OnEnter(Entity entity, CharacterChoiceStateMachineComponent stateMachine)
        {
            var state = stateMachine.State;
            switch (state.StateEnum)
            {
                case CharacterChoiceStates.Idle:
                    SetSpritePositions(entity, state, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetSpritePositions(Entity entity, CharacterChoiceState state, int percentAway)
        {
            var choice = (CharacterChoice)entity;
            var numChoices = state.CharacterOrder.Count;
            var gapBetween = 360f / numChoices;
            var positionOfChoice = state.CharacterOrder.FindIndex(c => c == state.CurrentChoice);
            for(var i = 0; i < numChoices; i++)
            {
                var character = state.CharacterOrder[i];
                var diff = i - positionOfChoice; // if you are to the left it's negative
                // 270 is pointing down
                var angle = 270 + (diff * gapBetween);
                var vector = HandyMath.MakeNormalizedVectorFromAngleDegrees(angle);
                vector = vector * CharacterChoiceState.DistanceFromCenter;
                var sprite = choice.CharacterChoiceSprites[character];
                sprite.setOrigin(choice.position + vector);
            }
        }

        protected override void OnExit(Entity entity, CharacterChoiceStateMachineComponent stateMachine)
        {
        }

    }
}