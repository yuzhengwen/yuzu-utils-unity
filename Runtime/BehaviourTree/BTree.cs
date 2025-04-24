using System.Collections.Generic;
using UnityEngine;

namespace YuzuValen.Utils.BehaviourTree
{
    public abstract class BTree : MonoBehaviour
    {
        private readonly Dictionary<string, object> data = new();

        private BTNode root;

        protected void Start()
        {
            root = SetUpTree();
        }

        protected virtual void Update()
        {
            if (GetData<BTNode>("PriorityOverride") != null)
                GetData<BTNode>("PriorityOverride").Evaluate();
            else
                root?.Evaluate();
        }

        public T GetData<T>(string key)
        {
            return data.TryGetValue(key, out var value) ? (T)value : default;
        }

        public void SetData<T>(string key, T value)
        {
            data[key] = value;
        }

        public void ClearData(string key)
        {
            data[key] = default;
        }

        protected abstract BTNode SetUpTree();
    }
}