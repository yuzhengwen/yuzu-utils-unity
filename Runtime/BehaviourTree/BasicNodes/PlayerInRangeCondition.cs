using UnityEngine;

namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    public class PlayerInRangeCondition : ConditionNode
    {
        private readonly float range;
        private readonly Transform transform;

        public PlayerInRangeCondition(BTree tree, Transform transform, float range) : base(tree)
        {
            this.transform = transform;
            this.range = range;
        }

        protected override bool CheckCondition()
        {
            var hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero);
            foreach (var hit in hits)
                if (hit.collider.CompareTag("Player"))
                {
                    tree.SetData("PlayerPosition", hit.collider.bounds.center);
                    return true;
                }

            return false;
        }
    }
}