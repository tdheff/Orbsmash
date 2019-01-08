using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace Handy.Components
{
    public class StateMachineComponent<TStateEnum, TState> : Component where TState : IStateMachineState<TStateEnum>
    {
        private readonly Stack<TState> _states;

        protected StateMachineComponent(TState initialState)
        {
            _states = new Stack<TState>();
            _states.Push(initialState);
        }

        [InspectableClass]
        public TState State
        {
            get => _states.Peek();
            set
            {
                _states.Pop();
                _states.Push(value);
            }
        }

        public override void debugRender(Graphics graphics)
        {
            var scale = entity.scale;
            if (_states.Count > 0)
                graphics.batcher.drawString(graphics.bitmapFont, $"{State.StateEnum}", entity.position, Color.OrangeRed,
                    0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void UpdateState(TState updatedState)
        {
            _states.Pop();
            _states.Push(updatedState);
        }

        public TState PopState()
        {
            return _states.Pop();
        }

        public void PushState(TStateEnum newState)
        {
            var next = State.Clone();
            next.StateEnum = newState;
            _states.Push((TState) next);
        }

        public TState ReplaceState(TStateEnum newState)
        {
            var previous = _states.Pop();
            var next = previous.Clone();
            next.StateEnum = newState;
            _states.Push((TState) next);
            return previous;
        }

        public void Clear()
        {
            _states.Clear();
        }
    }
}