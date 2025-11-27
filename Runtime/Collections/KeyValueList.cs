using System.Collections.Generic;

namespace YuzuValen.Utils.Collections
{
    [System.Serializable]
    public class KeyValueList<TKey, TValue>
    {
        public List<SerializableKeyValuePair<TKey, TValue>> pairs = new();

        public void Add(TKey key, TValue value)
        {
            pairs.Add(new SerializableKeyValuePair<TKey, TValue>(key, value));
        }

        public int Count => pairs.Count;
        public TValue this[TKey key] => pairs.Find(kvp => EqualityComparer<TKey>.Default.Equals(kvp.key, key)).value;
        public void Clear() => pairs.Clear();
        public bool ContainsKey(TKey key) => pairs.Exists(kvp => EqualityComparer<TKey>.Default.Equals(kvp.key, key));

        public bool Remove(TKey key)
        {
            var kvp = pairs.Find(k => EqualityComparer<TKey>.Default.Equals(k.key, key));
            return pairs.Remove(kvp);
        }
    }
}