namespace Handy.UI
{
    public class UIAction<TType, TData>
    {
        public UIAction(TType type, TData data)
        {
            Type = type;
            Data = data;
        }

        public UIAction(TType type)
        {
            Type = type;
        }

        public TType Type { get; }
        public TData Data { get; }
    }
}