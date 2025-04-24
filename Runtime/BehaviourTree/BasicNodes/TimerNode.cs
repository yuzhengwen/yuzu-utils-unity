using UnityEngine;

namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    public class TimerNode : BTNode, IResetable
    {
        private readonly float duration;
        private float timer;

        public TimerNode(BTree bTree, float duration) : base(bTree)
        {
            this.duration = duration;
        }

        public void Reset()
        {
            timer = 0;
        }

        public override BTNodeState Evaluate()
        {
            if (timer >= duration) return BTNodeState.Success;

            timer += Time.deltaTime;
            return BTNodeState.Running;
        }
    }
}