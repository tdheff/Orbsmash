using Optional;

namespace Handy.Dispatch
{
    public interface IDispatcher
    {
        /// <summary>
        ///     returns the next action
        /// </summary>
        Option<IAction> Peek();

        /// <summary>
        ///     removes the next action and returns it
        /// </summary>
        Option<IAction> Pop();

        /// <summary>
        ///     returns the next action of type T
        /// </summary>
        Option<T> Peek<T>() where T : IAction;

        /// <summary>
        ///     removes the next action of type T and returns it
        /// </summary>
        Option<T> Pop<T>() where T : IAction;

        /// <summary>
        ///     dispatches an action
        /// </summary>
        void Dispatch(IAction action);
    }
}