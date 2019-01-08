namespace Nez.AI.BehaviorTrees
{
	/// <summary>
	///     root class used to control a BehaviorTree. Handles storing the context
	/// </summary>
	public class BehaviorTree<T>
    {
	    /// <summary>
	    ///     The context should contain all the data needed to run the tree
	    /// </summary>
	    private readonly T _context;

        private float _elapsedTime;

        /// <summary>
        ///     root node of the tree
        /// </summary>
        private readonly Behavior<T> _root;

        /// <summary>
        ///     how often the behavior tree should update. An updatePeriod of 0.2 will make the tree update 5 times a second.
        /// </summary>
        public float updatePeriod;


        public BehaviorTree(T context, Behavior<T> rootNode, float updatePeriod = 0.2f)
        {
            _context = context;
            _root = rootNode;

            this.updatePeriod = _elapsedTime = updatePeriod;
        }


        public void tick()
        {
            // updatePeriod less than or equal to 0 will tick every frame
            if (updatePeriod > 0)
            {
                _elapsedTime -= Time.deltaTime;
                if (_elapsedTime <= 0)
                {
                    // ensure we only tick once for long frames
                    while (_elapsedTime <= 0)
                        _elapsedTime += updatePeriod;

                    _root.tick(_context);
                }
            }
            else
            {
                _root.tick(_context);
            }
        }
    }
}