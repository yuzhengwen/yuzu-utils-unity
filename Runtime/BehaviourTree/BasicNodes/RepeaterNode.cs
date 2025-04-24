namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    public class RepeaterNode : BTNode, IResetable
    {
        private readonly BTNode node;
        private readonly int targetCount;
        private int count;

        public RepeaterNode(BTree tree, BTNode node, int targetCount) : base(tree)
        {
            this.node = node;
            this.targetCount = targetCount;
        }

        public void Reset()
        {
            count = 0;
            if (node is IResetable resetable) resetable.Reset();
        }

        public override BTNodeState Evaluate()
        {
            if (count == targetCount) return BTNodeState.Success;

            var childState = node.Evaluate();
            if (childState == BTNodeState.Success)
            {
                (node as IResetable)?.Reset();
                count++;
                if (count == targetCount) return BTNodeState.Success;
            }

            return BTNodeState.Running;
        }
    }
}