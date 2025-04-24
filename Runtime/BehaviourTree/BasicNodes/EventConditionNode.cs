using System;

namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    /// <summary>
    ///     Returns success if the event is triggered for one frame.
    /// </summary>
    public class EventConditionNode : BTNode
    {
        private bool eventTriggered;

        public EventConditionNode(BTree tree, ref Action triggerEvent) : base(tree)
        {
            triggerEvent += OnEventTrigger;
        }

        private void OnEventTrigger()
        {
            eventTriggered = true;
        }

        private bool CheckCondition()
        {
            return eventTriggered;
        }

        public override BTNodeState Evaluate()
        {
            if (CheckCondition())
            {
                eventTriggered = false;
                return BTNodeState.Success;
            }

            return BTNodeState.Failure;
        }
    }
}