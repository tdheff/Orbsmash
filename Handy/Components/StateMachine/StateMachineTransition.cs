namespace Handy.Components
{
    public enum TransitionTypes
    {
        None,
        Replace,
        Push,
        Pop
    }

    public class StateMachineTransition<TStateEnum>
    {
        public TransitionTypes TransitionType { get; private set; }
        public TStateEnum NextState { get; private set; }

        public static StateMachineTransition<TStateEnum> None()
        {
            return new StateMachineTransition<TStateEnum>()
            {
                TransitionType = TransitionTypes.None
            };
        }
        
        public static StateMachineTransition<TStateEnum> Pop()
        {
            return new StateMachineTransition<TStateEnum>()
            {
                TransitionType = TransitionTypes.Pop
            };
        }
        
        public static StateMachineTransition<TStateEnum> Replace(TStateEnum nextState)
        {
            return new StateMachineTransition<TStateEnum>()
            {
                TransitionType = TransitionTypes.Replace,
                NextState = nextState
            };
        }
        
        public static StateMachineTransition<TStateEnum> Push(TStateEnum nextState)
        {
            return new StateMachineTransition<TStateEnum>()
            {
                TransitionType = TransitionTypes.Push,
                NextState = nextState
            };
        }
    }
}