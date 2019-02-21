using System;
using System.Collections.Generic;
using Handy.Components;
using Nez;

namespace Handy.Systems
{
    public abstract class PushdownAutomatonSystem<TStateEnum, TState, TStateMachineComponent> : EntitySystem
        where TStateMachineComponent : StateMachineComponent<TStateEnum, TState>
        where TState : IStateMachineState<TStateEnum>
    {   
        protected PushdownAutomatonSystem(Matcher matcher) : base(matcher) { }
        
        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var stateMachine = entity.getComponent<TStateMachineComponent>();

                Update(entity, stateMachine);
                
                var transition = Transition(entity, stateMachine);

                switch (transition.TransitionType)
                {
                    case TransitionTypes.None:
                        break;
                    case TransitionTypes.Replace:
                        OnExit(entity, stateMachine);
                        stateMachine.ReplaceState(transition.NextState);
                        OnEnter(entity, stateMachine);
                        break;
                    case TransitionTypes.Push:
                        OnSuspend(entity, stateMachine);
                        stateMachine.PushState(transition.NextState);
                        OnEnter(entity, stateMachine);
                        break;
                    case TransitionTypes.Pop:
                        OnExit(entity, stateMachine);
                        var current = stateMachine.PopState();
                        var nextEnum = stateMachine.PeekState().StateEnum;
                        current.StateEnum = nextEnum;
                        stateMachine.UpdateState(current);
                        OnWakeup(entity, stateMachine);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        ///  <summary>
        ///  Updates character state. Called before transition.
        /// 
        ///  </summary>
        /// <param name="entity">The entity that has this state</param>
        /// <param name="stateMachine">The state machine being operated on</param>
        protected abstract void Update(Entity entity, TStateMachineComponent stateMachine);

        ///  <summary>
        ///  Decides what state should be next, and returns the appropriate transition.
        /// 
        ///  This method should not modify the outgoing state, instead that should be done in <see cref="OnExit"/>
        ///  </summary>
        /// <param name="entity">The entity that has this state</param>
        /// <param name="stateMachine">The state machine being operated on</param>
        ///  <returns>A transition to a new state</returns>
        protected abstract StateMachineTransition<TStateEnum> Transition(Entity entity, TStateMachineComponent stateMachine);

        /// <summary>
        /// Called when the current state will be permanently exited and removed from the stack
        /// </summary>
        /// <param name="entity">The entity that has this state</param>
        /// <param name="stateMachine">The state machine BEFORE the transition</param>
        protected virtual void OnExit(Entity entity, TStateMachineComponent stateMachine) { }

        /// <summary>
        /// Called when the current state is being entered and has been added to the stack
        /// </summary>
        /// <param name="entity">The entity that has this state</param>
        /// <param name="stateMachine">The state machine AFTER the transition</param>
        protected virtual void OnEnter(Entity entity, TStateMachineComponent stateMachine) { }
        
        /// <summary>
        /// Called when a state will be pushed down by a new state
        /// </summary>
        /// <param name="entity">The entity that has this state</param>
        /// <param name="stateMachine">The state machine AFTER the transition</param>
        protected virtual void OnSuspend(Entity entity, TStateMachineComponent stateMachine) { }
        
        /// <summary>
        /// Called when a state has been activated by virtue of the state in front of it being popped off the stack
        /// </summary>
        /// <param name="entity">The entity that has this state</param>
        /// <param name="stateMachine">The state machine AFTER the transition</param>
        protected virtual void OnWakeup(Entity entity, TStateMachineComponent stateMachine) { }

    }

}