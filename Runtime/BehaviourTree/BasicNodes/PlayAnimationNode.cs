using UnityEngine;

namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    public class PlayAnimationNode : BTNode, IResetable
    {
        private readonly string anim;
        private readonly Animator animator;
        private readonly float duration;
        private float timer;

        public PlayAnimationNode(BTree tree, Animator animator, string anim, float duration) : base(tree)
        {
            this.animator = animator;
            this.anim = anim;
            this.duration = duration;
        }

        public void Reset()
        {
            timer = 0;
        }

        public override BTNodeState Evaluate()
        {
            if (timer == 0)
                animator.SetTrigger(anim);
            timer += Time.deltaTime;
            if (timer >= duration) return BTNodeState.Success;

            return BTNodeState.Running;
        }
    }
}