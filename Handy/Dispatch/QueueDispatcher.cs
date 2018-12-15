using System;
using System.Collections.Generic;
using System.Linq;
using Nez.AI.UtilityAI;
using Optional;

namespace Handy.Dispatch
{
    public class QueueDispatcher : IDispatcher
    {
        private readonly List<IAction> _actions;
        
        public QueueDispatcher()
        {
            _actions = new List<IAction>();
        }

        public Option<IAction> Peek()
        {
            return _actions.Count > 0 ? Option.Some(_actions.First()) : Option.None<IAction>();
        }

        public Option<IAction> Pop()
        {
            if (_actions.Count > 0)
            {
                var action = _actions.First();
                _actions.RemoveAt(0);
                return Option.Some(action);
            } else {
                return Option.None<IAction>();
            }
        }

        public Option<T> Peek<T>() where T : IAction
        {
            foreach (var action in _actions)
            {
                if (!(action is T variable)) continue;
                return Option.Some(variable);
            }

            return Option.None<T>();
        }

        public Option<T> Pop<T>() where T : IAction
        {
            foreach (var action in _actions)
            {
                if (!(action is T variable)) continue;
                _actions.Remove(action);
                return Option.Some(variable);
            }

            return Option.None<T>();
        }

        public void Dispatch(IAction action)
        {
            _actions.Insert(0, action);
        }
    }
}