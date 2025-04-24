using UnityEngine;

namespace YuzuValen.Utils.BehaviourTree.BasicNodes
{
    public class CooldownConditionNode : ConditionNode
    {
        private readonly float cooldown;
        private bool firstTime = true;
        private float lastTime;

        public CooldownConditionNode(BTree tree, float cooldown) : base(tree)
        {
            this.cooldown = cooldown;
        }

        protected override bool CheckCondition()
        {
            if (Time.time - lastTime > cooldown || firstTime)
            {
                // if cooldown has passed or first time
                Debug.Log("<color=green>Cooldown passed</color>");
                firstTime = false;
                lastTime = Time.time;
                return true;
            }

            return false;
        }
    }
}