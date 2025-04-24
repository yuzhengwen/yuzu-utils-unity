namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    /// <summary>
    ///     Runs all children sequentially left to right <br />
    ///     Returns success if ALL children return success <br />
    ///     Stops if ANY child returns failure or running
    /// </summary>
    public class SequenceNode : BTNode, IResetable
    {
        /// <summary>
        ///     Remember the child that was running <br />
        ///     When the node is re-evaluated, it starts from the child it stopped at
        /// </summary>
        public bool rememberRunningChild = false;

        /// <summary>
        ///     Reset all children when the node returns success (All nodes returned success)
        /// </summary>
        public bool resetChildrenOnSuccess = true;

        private int runningChildIndex = -1;
        private string runningFlag;
        public bool setPriorityWhileRunning = false;

        public SequenceNode(BTree tree, params BTNode[] children) : base(tree, children)
        {
        }

        public void Reset()
        {
            foreach (var child in children)
                if (child is IResetable resettable)
                    resettable.Reset();
        }

        public override BTNodeState Evaluate()
        {
            if (runningFlag != null)
                tree.SetData(runningFlag, true);
            if (setPriorityWhileRunning)
                tree.SetData("PriorityOverride", this);

            var startingIndex = 0;
            // if we have a running child, start from that child
            if (rememberRunningChild && runningChildIndex != -1)
                startingIndex = runningChildIndex;

            for (var i = startingIndex; i < children.Count; i++)
            {
                var childState = children[i].Evaluate();
                if (childState == BTNodeState.Running)
                {
                    runningChildIndex = i;
                    return BTNodeState.Running;
                }

                runningChildIndex = -1; // if node success or failure, reset runningChildIndex

                if (childState == BTNodeState.Failure) return BTNodeState.Failure;
                if (childState == BTNodeState.Success) continue;
            }

            // if we reach here, all children returned success

            if (resetChildrenOnSuccess)
                Reset();

            if (runningFlag != null)
                tree.SetData(runningFlag, false);
            if (setPriorityWhileRunning)
                tree.ClearData("PriorityOverride");
            return BTNodeState.Success;
        }

        public void EnableFlagWhenChildrenAreRunning(string flag)
        {
            runningFlag = flag;
        }
    }

    public interface IResetable
    {
        void Reset();
    }
}