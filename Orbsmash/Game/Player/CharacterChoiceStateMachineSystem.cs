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
            typeof(PlayerInputComponent)
        )) { }

        protected override void Update(Entity entity, CharacterChoiceStateMachineComponent stateMachine)
        {
            var choice = (CharacterChoice)entity;
            choice.SetSpritePositionsFromCurrentState();
        }

        protected override StateMachineTransition<CharacterChoiceStates> Transition(Entity entity, CharacterChoiceStateMachineComponent stateMachine)
        {
            var input = entity.getComponent<PlayerInputComponent>();
            var state = stateMachine.State;
            if (state.StateEnum == CharacterChoiceStates.Idle && Math.Abs(input.MovementStick.X) > CharacterChoiceState.MOVEMENT_THRESHOLD)
            {
                if(input.MovementStick.X > 0)
                {
                    Console.WriteLine($"rotating right!");
                    return StateMachineTransition<CharacterChoiceStates>.Replace(CharacterChoiceStates.RotatingRight);

                } else
                {
                    Console.WriteLine($"rotating left!");
                    return StateMachineTransition<CharacterChoiceStates>.Replace(CharacterChoiceStates.RotatingLeft);
                }
            } else if (state.StateEnum == CharacterChoiceStates.RotatingLeft || state.StateEnum == CharacterChoiceStates.RotatingRight)
            {
                if(Time.time - state.LastRotatedTime > CharacterChoiceState.ROTATION_TIME)
                {
                    Console.WriteLine($"done rotating!! back to idle");
                    return StateMachineTransition<CharacterChoiceStates>.Replace(CharacterChoiceStates.Idle); // back into idle
                }
            }
            return StateMachineTransition<CharacterChoiceStates>.None();
        }

        protected override void OnEnter(Entity entity, CharacterChoiceStateMachineComponent stateMachine)
        {
            var choice = (CharacterChoice)entity;
            var state = stateMachine.State;
            var input = entity.getComponent<PlayerInputComponent>();
            var indexOfCurrentChoice = state.CharacterOrder.FindIndex(c => c == state.CurrentChoice);
            Console.WriteLine($"setting next choice: current choice: {indexOfCurrentChoice}");

            if (state.StateEnum == CharacterChoiceStates.RotatingLeft || state.StateEnum == CharacterChoiceStates.RotatingRight)
            {
                indexOfCurrentChoice = state.StateEnum == CharacterChoiceStates.RotatingRight ? indexOfCurrentChoice + 1 : indexOfCurrentChoice - 1;
                if (indexOfCurrentChoice < 0)
                {
                    indexOfCurrentChoice = state.CharacterOrder.Count - 1;
                }
                else if (indexOfCurrentChoice == state.CharacterOrder.Count)
                {
                    indexOfCurrentChoice = 0;
                }
                Console.WriteLine($"setting next choice: NEXT choice: {indexOfCurrentChoice}");
                state.CurrentChoice = state.CharacterOrder[indexOfCurrentChoice];
                state.LastRotatedTime = Time.time;
                choice.SetRotationAngleToMoveTowardsCurrentChoice();
            }
        }

        protected override void OnExit(Entity entity, CharacterChoiceStateMachineComponent stateMachine)
        {
        }

    }
}