using System.Collections.Generic;

namespace YuzuValen.Utils.BehaviourTree
{
    public abstract class BTNode
    {
        public List<BTNode> children = new();
        public BTNode parent;

        protected BTree tree;

        public BTNode(BTree tree, params BTNode[] children)
        {
            this.tree = tree;
            foreach (var child in children)
                AttachChild(child);
        }

        public void AttachChild(BTNode child)
        {
            child.parent = this;
            children.Add(child);
        }

        public abstract BTNodeState Evaluate();

        protected T GetData<T>(string key)
        {
            return tree.GetData<T>(key);
        }

        protected void SetData(string key, object value)
        {
            tree.SetData(key, value);
        }

        protected void ClearData(string key)
        {
            tree.ClearData(key);
        }
    }

    public enum BTNodeState
    {
        Success,
        Running,
        Failure
    }

    public class BTCompositeNodeSettings
    {
        public bool randomize;
    }
}