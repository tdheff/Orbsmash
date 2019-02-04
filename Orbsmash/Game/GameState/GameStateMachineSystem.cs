using System;
using Handy.Components;
using Handy.Systems;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Ball;
using Orbsmash.Constants;
using Orbsmash.Player;
using Random = Nez.Random;

namespace Orbsmash.Game
{
    public class GameStateMachineSystem : StateMachineSystem<GameStates, GameState, GameStateComponent>
    {
        public GameStateMachineSystem() : base(new Matcher().all(typeof(GameStateComponent))) { }
        
        protected override void Update(Entity entity, GameStateComponent stateMachine)
        {
        }

        protected override StateMachineTransition<GameStates> Transition(Entity entity, GameStateComponent stateMachine)
        {
            var state = stateMachine.State;
            var gameStateTimer = entity.getComponent<TimerComponent>();
            switch (state.StateEnum)
            {
                case GameStates.Ready:
                    return StateMachineTransition<GameStates>.Replace(GameStates.Service);
                case GameStates.Service:
                    if (!state.Ball.getComponent<VelocityComponent>().Freeze)
                    {
                        return StateMachineTransition<GameStates>.Replace(GameStates.Play);
                    }
                    break;
                case GameStates.Play:
                    var knockedOutTeam = isOneTeamKnockedOut(state.Players);
                    switch (knockedOutTeam)
                    {
                        case Gameplay.Side.LEFT:
                            return StateMachineTransition<GameStates>.Replace(GameStates.PointScoredRight);
                        case Gameplay.Side.RIGHT:
                            return StateMachineTransition<GameStates>.Replace(GameStates.PointScoredLeft);
                        case Gameplay.Side.NONE:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case GameStates.PointScoredRight:
                case GameStates.PointScoredLeft:
                    if (gameStateTimer.Finished)
                    {
                        return StateMachineTransition<GameStates>.Replace(GameStates.Service);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return StateMachineTransition<GameStates>.None();
        }

        private void ResetPlayers(Player.Player[] players)
        {
            foreach (var player in players)
            {
                var state = player.getComponent<PlayerStateComponent>();
                state.IsKilled = false;
                player.position = state.ResetPosition;
            }
        }

        private void ResetBall(Ball.Ball ball, Gameplay.Side side)
        {
            var ballState = ball.getComponent<BallStateComponent>();
            ballState.IsDeadly = false;
            ballState.BaseSpeed = ballState.BaseSpeedInitial;
            if (side == Gameplay.Side.LEFT ||
                (side == Gameplay.Side.NONE )) // && Random.chance(0.5f)
            {
                ball.position = BallResetPositions.LEFT_RESET;
            }
            else
            {
                ball.position = BallResetPositions.RIGHT_RESET;
            }

            ball.getComponent<VelocityComponent>().Freeze = true;
        }
        
        private Gameplay.Side isOneTeamKnockedOut(Player.Player[] players)
        {
            int rightAlive = 0;
            int leftAlive = 0;

            foreach (var player in players)
            {
                var state = player.getComponent<PlayerStateComponent>();
                if (state.StateEnum != KnightStates.Dead)
                {
                    if (state.side == Gameplay.Side.LEFT)
                    {
                        leftAlive++;
                    }
                    else
                    {
                        rightAlive++;
                    }
                }
            }
            
            if (rightAlive == 0)
            {
                return Gameplay.Side.RIGHT;
            }

            if (leftAlive == 0)
            {
                return Gameplay.Side.LEFT;
            }
            return Gameplay.Side.NONE;
        }

        protected override void OnExit(Entity entity, GameStateComponent stateMachine)
        {
            var state = stateMachine.State;
            var gameStateTimer = entity.getComponent<TimerComponent>();
            switch (state.StateEnum)
            {
                case GameStates.Ready:
                    ResetBall(state.Ball, Gameplay.Side.NONE);
                    break;
                case GameStates.Service:
                    break;
                case GameStates.Play:
                    break;
                case GameStates.PointScoredLeft:
                    ResetBall(state.Ball, Gameplay.Side.LEFT);
                    break;
                case GameStates.PointScoredRight:
                    ResetBall(state.Ball, Gameplay.Side.LEFT);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnEnter(Entity entity, GameStateComponent stateMachine)
        {
            var state = stateMachine.State;
            var gameStateTimer = entity.getComponent<TimerComponent>();
            switch (state.StateEnum)
            {
                case GameStates.Ready:
                    break;
                case GameStates.Service:
                    ResetPlayers(state.Players);
                    break;
                case GameStates.Play:
                    break;
                case GameStates.PointScoredRight:
                    gameStateTimer.Set(Timers.POINT_SCORED_TIMER);
                    state.LeftPoints++;
                    break;
                case GameStates.PointScoredLeft:
                    gameStateTimer.Set(Timers.POINT_SCORED_TIMER);
                    state.RightPoints++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}