using UnityEngine;

namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    public class InverterNode : BTNode
    {
        private readonly BTNode child;

        public InverterNode(BTree tree, BTNode child) : base(tree)
        {
            this.child = child;
        }

        public override BTNodeState Evaluate()
        {
            var childState = child.Evaluate();
            if (childState != BTNodeState.Running)
                return childState == BTNodeState.Success ? BTNodeState.Failure : BTNodeState.Success;
            Debug.LogWarning("Inverter node should not have running state");
            return childState;
        }
    }
}