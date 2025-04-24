using System;
using System.Collections.Generic;
using UnityEngine;

namespace YuzuValen.Utils
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<TValue> values = new List<TValue>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();
            if (keys.Count != values.Count)
            {
                UnityEngine.Debug.LogWarning("The number of keys and values does not match!");
            }

            for (int i = 0; i < keys.Count; i++)
            {
                this[keys[i]] = values.Count==keys.Count ? values[i] : default;
            }
        }
    }

    // my own implementation of serializable dictionary using a custom serializable class. Able to work in scriptable objects!
    [Serializable]
    public class YDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<YDictionaryEntry<TKey, TValue>> entries = new();

        public void OnBeforeSerialize()
        {
            entries.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                entries.Add(new YDictionaryEntry<TKey, TValue>(pair.Key, pair.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            foreach (var entry in entries)
            {
#if UNITY_EDITOR
                if (!ContainsKey(entry.key))
                    Add(entry.key, entry.value);
#else
                    Add(entry.key, entry.value);
#endif
            }
        }
    }

    [Serializable]
    public class YDictionaryEntry<TKey, TValue>
    {
        public TKey key;
        public TValue value;

        public YDictionaryEntry(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }
}