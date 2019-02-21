using System;
using Handy.Components;
using Handy.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Orbsmash.Ball;
using Orbsmash.Constants;
using Orbsmash.Player;
using Random = Nez.Random;

namespace Orbsmash.Game
{
    public class PracticeGameStateMachineSystem : PushdownAutomatonSystem<PracticeGameStates, PracticeGameState, PracticeGameStateComponent>
    {
        public PracticeGameStateMachineSystem() : base(new Matcher().all(typeof(PracticeGameStateComponent))) { }
        
        protected override void Update(Entity entity, PracticeGameStateComponent stateMachine)
        {
            var state = stateMachine.State;
            var practiceGame = (PracticeGame) scene;
            // we allow max 4 players at this point
            for (var i = 0; i < 4 && i < Input.gamePads.Length; i++)
            {
                var gamePad = Input.gamePads[i];
                var playerState = state.PlayerStates[i];
                if(playerState == PracticeGamePlayerReadyState.NotJoined)
                {
                    if (gamePad.isButtonPressed(Buttons.A))
                    {
                        state.PlayerStates[i] = PracticeGamePlayerReadyState.Choosing;
                        AddChooser(i, state);
                    }
                } else if (playerState == PracticeGamePlayerReadyState.Choosing)
                {
                    if (gamePad.isButtonPressed(Buttons.A))
                    {
                        state.PlayerStates[i] = PracticeGamePlayerReadyState.HangingOut;
                        AddPlayer(i, state);
                    }
                }
            }
            // TODO - add "backing out" on a per player basis (e.g. press B to go back to chooser)
        }

        public void AddChooser(int id, PracticeGameState state)
        {
            var practiceGame = (PracticeGame)scene;
            // TODO - make this actually random!
            var randomPosition = new Vector2(1345, 450);
            var chooser = new CharacterChoice(id, Gameplay.Character.KNIGHT, randomPosition);
            state.Choosers[id] = chooser;
            practiceGame.addEntity(chooser);
        }

        public void AddPlayer(int id, PracticeGameState state)
        {
            var practiceGame = (PracticeGame)scene;
            var chooser = state.Choosers[id];
            var choice = chooser._stateComponent.State.CurrentChoice;
            var player = new Player.Player(new PlayerSettings
            {
                Id = id,
                Side = Gameplay.Side.LEFT, // doesn't matter!
                Speed = 300f,
                StartingPosition = chooser.position,
                Character = choice
            });
            practiceGame.addEntity(player);
            chooser.destroy();
        }

        protected override StateMachineTransition<PracticeGameStates> Transition(Entity entity, PracticeGameStateComponent stateMachine)
        {
            return StateMachineTransition<PracticeGameStates>.None();
            // TODO ADD CHECKS FOR WHEN ALL PLAYERS JOINED
            // AND FOR WHEN ALL PLAYERS ARE ON ZONES OR NOT
        }

        protected override void OnExit(Entity entity, PracticeGameStateComponent stateMachine)
        {
            
        }

        protected override void OnEnter(Entity entity, PracticeGameStateComponent stateMachine)
        {
            
        }
    }
}