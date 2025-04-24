namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    public class ConditionalCompositeNode : BTNode
    {
        private readonly BTNode condition, actionSuccess, actionFail;

        public ConditionalCompositeNode(BTree tree, BTNode condition, BTNode actionSuccess, BTNode actionFail) :
            base(tree)
        {
            this.condition = condition;
            this.actionSuccess = actionSuccess;
            this.actionFail = actionFail;
        }

        public override BTNodeState Evaluate()
        {
            if (condition.Evaluate() == BTNodeState.Success)
                if (actionSuccess != null)
                    return actionSuccess.Evaluate();

            if (actionFail != null) return actionFail.Evaluate();

            return BTNodeState.Failure;
        }
    }
}