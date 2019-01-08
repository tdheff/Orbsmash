namespace Nez.AI.BehaviorTrees
{
	/// <summary>
	///     runs an entire BehaviorTree as a child and returns success
	/// </summary>
	public class BehaviorTreeReference<T> : Behavior<T>
    {
        private readonly BehaviorTree<T> _childTree;


        public BehaviorTreeReference(BehaviorTree<T> tree)
        {
            _childTree = tree;
        }


        public override TaskStatus update(T context)
        {
            _childTree.tick();
            return TaskStatus.Success;
        }
    }
}