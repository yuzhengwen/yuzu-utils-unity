namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    public abstract class ConditionNode : BTNode
    {
        protected ConditionNode(BTree tree) : base(tree)
        {
        }

        protected abstract bool CheckCondition();

        public override BTNodeState Evaluate()
        {
            return CheckCondition() ? BTNodeState.Success : BTNodeState.Failure;
        }
    }
}