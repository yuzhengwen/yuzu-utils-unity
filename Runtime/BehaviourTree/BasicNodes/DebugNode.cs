using UnityEngine;

namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    public class DebugNode : BTNode
    {
        private readonly string message;

        public DebugNode(string message = "Debug Node Reached") : base(null)
        {
            this.message = message;
        }

        public override BTNodeState Evaluate()
        {
            Debug.Log(message);
            return BTNodeState.Success;
        }
    }
}