namespace Handy.UI
{
    public class UIAction<TType, TData>
    {
        public TType Type { get; }
        public TData Data { get; }
        
        public UIAction(TType type, TData data)
        {
            Type = type;
            Data = data;
        }

        public UIAction(TType type)
        {
            Type = type;
        }
    }
}