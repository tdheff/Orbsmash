namespace Handy.Components
{
    public interface IStateMachineState<TStateEnum>
    {
        TStateEnum StateEnum { get; set; }

        IStateMachineState<TStateEnum> Clone();
    }
}