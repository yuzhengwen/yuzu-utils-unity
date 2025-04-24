using YuzuValen.Utils;

namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    /// <summary>
    ///     Runs all children left to right <br />
    ///     Returns success if ANY children returns success <br />
    ///     Returns running if ANY children returns running <br />
    ///     Returns failure if ALL children returns failure <br />
    /// </summary>
    public class SelectorNode : BTNode
    {
        /// <summary>
        ///     Randomize order of execution of children each time a child is successfully selected (returns success)
        /// </summary>
        public bool randomizeExecutionOrder = false;

        /// <summary>
        ///     Remember the child that was running <br />
        ///     When the node is re-evaluated, it starts from the child it stopped at
        /// </summary>
        public bool rememberRunningChild = false;

        private int runningChildIndex = -1;

        public SelectorNode(BTree tree, params BTNode[] children) : base(tree, children)
        {
            if (randomizeExecutionOrder)
                this.children.Shuffle();
        }

        public override BTNodeState Evaluate()
        {
            // if we have a running child, start from that child
            var startingIndex = rememberRunningChild && runningChildIndex != -1 ? runningChildIndex : 0;
            for (var i = startingIndex; i < children.Count; i++)
            {
                var childState = children[i].Evaluate();
                if (childState == BTNodeState.Running)
                {
                    runningChildIndex = i;
                    return BTNodeState.Running;
                }

                runningChildIndex = -1; // if node success or failure, reset runningChildIndex

                if (childState == BTNodeState.Failure) continue;
                if (childState == BTNodeState.Success)
                {
                    if (randomizeExecutionOrder)
                        children.Shuffle();
                    return BTNodeState.Success;
                }
            }

            return BTNodeState.Failure;
        }
    }
}