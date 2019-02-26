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
                    var ballVelocity = state.Ball.getComponent<VelocityComponent>();
                    var ballState = state.Ball.getComponent<BallStateComponent>();
                    if (!ballVelocity.Freeze)
                    {
                        ballVelocity.Velocity = ballVelocity.Velocity / ballState.HitBoost;
                        ballState.HitBoost = 1.0f;
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

        private void ResetForService(Ball.Ball ball, Player.Player[] players, Gameplay.Side side)
        {
            ResetBall(ball, side);
            ResetPlayers(players);
        }

        private void ResetBall(Ball.Ball ball,  Gameplay.Side side)
        {
            var ballState = ball.getComponent<BallStateComponent>();
            ballState.LastHitSide = Gameplay.Side.NONE;
            ballState.HitBoost = 1.0f;
            ballState.IsDeadly = false;
            ballState.BaseSpeed = ballState.BaseSpeedInitial;
            ballState.IsBeingServed = true;
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

        private void ResetPlayers(Player.Player[] players)
        {
            foreach (var player in players)
            {
                var state = player.getComponent<PlayerStateComponent>();
                state.IsKilled = false;
                player.position = state.ResetPosition;
            }
        }

        private Gameplay.Side isOneTeamKnockedOut(Player.Player[] players)
        {
            int rightAlive = 0;
            int leftAlive = 0;

            foreach (var player in players)
            {
                var isAlive = false;
                switch (player.Settings.Character)
                {
                    case Gameplay.Character.KNIGHT:
                        var knightState = player.getComponent<KnightStateMachineComponent>().State.StateEnum;
                        isAlive = knightState != KnightStates.Eliminated;
                        break;
                    case Gameplay.Character.WIZARD:
                        var wizardState = player.getComponent<WizardStateMachineComponent>().State.StateEnum;
                        isAlive = wizardState != WizardStates.Eliminated;
                        break;
                    case Gameplay.Character.SPACEMAN:
                        var spacemanState = player.getComponent<SpacemanStateMachineComponent>().State.StateEnum;
                        isAlive = spacemanState != SpacemanStates.KO && spacemanState != SpacemanStates.Eliminated;
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
                if (isAlive)
                {
                    if (player.Settings.Side == Gameplay.Side.LEFT)
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
                    break;
                case GameStates.Service:
                    break;
                case GameStates.Play:
                    
                    break;
                case GameStates.PointScoredLeft:
                    break;
                case GameStates.PointScoredRight:
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
                    var knockedOutTeam = isOneTeamKnockedOut(state.Players);
                    if (knockedOutTeam == Gameplay.Side.NONE)
                    {
                        ResetForService(state.Ball, state.Players, Gameplay.Side.NONE);
                    }
                    else
                    {
                        ResetForService(state.Ball, state.Players, knockedOutTeam == Gameplay.Side.LEFT ? Gameplay.Side.RIGHT : Gameplay.Side.LEFT);
                    }
                    break;
                case GameStates.Play:
                    break;
                case GameStates.PointScoredRight:
                    gameStateTimer.Set(Timers.POINT_SCORED_TIMER);
                    state.RightPoints++;
                    break;
                case GameStates.PointScoredLeft:
                    gameStateTimer.Set(Timers.POINT_SCORED_TIMER);
                    state.LeftPoints++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}