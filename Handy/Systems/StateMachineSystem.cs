using System;
using System.Collections.Generic;
using Handy.Components;
using Nez;

namespace Handy.Systems
{
    public abstract class
        StateMachineSystem<TStateEnum, TState, TStateMachineComponent> : EntitySystem
        where TStateMachineComponent : StateMachineComponent<TStateEnum, TState>
        where TState : IStateMachineState<TStateEnum>
    {
        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var stateMachine = entity.getComponent<TStateMachineComponent>();

                var transition = Transition(entity, stateMachine);

                if (!transition.Equals(stateMachine.State.StateEnum))
                {
                    OnExit(entity, stateMachine);
                    stateMachine.ReplaceState(transition);
                    OnEnter(entity, stateMachine);
                }
            }
        }
        
        ///  <summary>
        ///  Decides what state should be next, and returns the new state type.
        /// 
        ///  This method should not modify the outgoing state, instead that should be done in <see cref="OnExit"/>
        ///  </summary>
        /// <param name="entity">The entity that has this state</param>
        /// <param name="stateMachine">The state machine being operated on</param>
        ///  <returns>A transition to a new state</returns>
        protected abstract TStateEnum Transition(Entity entity, TStateMachineComponent stateMachine);
        
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
    }
}