namespace Nez.AI.UtilityAI
{
    public class UtilityAI<T>
    {
	    /// <summary>
	    ///     The context should contain all the data needed to run the tree
	    /// </summary>
	    private readonly T _context;

        private float _elapsedTime;

        private readonly Reasoner<T> _rootReasoner;

        /// <summary>
        ///     how often the behavior tree should update. An updatePeriod of 0.2 will make the tree update 5 times a second.
        /// </summary>
        public float updatePeriod;


        public UtilityAI(T context, Reasoner<T> rootSelector, float updatePeriod = 0.2f)
        {
            _rootReasoner = rootSelector;
            _context = context;
            this.updatePeriod = _elapsedTime = updatePeriod;
        }


        public void tick()
        {
            _elapsedTime -= Time.deltaTime;
            while (_elapsedTime <= 0)
            {
                _elapsedTime += updatePeriod;
                var action = _rootReasoner.select(_context);
                if (action != null)
                    action.execute(_context);
            }
        }
    }
}