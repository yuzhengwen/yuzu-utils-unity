using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace YuzuValen.Utils.Collections
{
    [System.Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        public TKey key;
        public TValue value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> pairs = new();

        public void OnBeforeSerialize()
        {
            // dictionary -> inspector list (serialized)
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // should only run in play mode, else it'll overwrite any changes made in the inspector
                SerializeFromDictionary();
            }
#else
              SerializeFromDictionary();
#endif
        }

        /// <summary>
        /// Uses JsonUtility to serialize the dictionary to a JSON string
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            SerializeFromDictionary();
            return JsonUtility.ToJson(this);
        }

        /// <summary>
        /// Serializes the dictionary into the pairs list (for serialization)
        /// </summary>
        public void SerializeFromDictionary()
        {
            pairs.Clear();
            foreach (var kvp in this)
            {
                pairs.Add(new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            // inspector list (serialized) -> dictionary
            Clear();
            for (int i = 0; i < pairs.Count; i++)
            {
                var pair = pairs[i];
                this[pair.key] = pair.value;
            }
        }
    }
}