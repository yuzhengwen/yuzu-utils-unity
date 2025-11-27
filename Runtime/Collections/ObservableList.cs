using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YuzuValen.Utils.Collections
{
    [System.Serializable]
    public class ObservableList<T>
    {
        // wrapping a list instead of inheriting in order to allow Unity serialization
        [SerializeField] private List<T> list = new List<T>();

        public delegate void
            ObservableListChangedHandler<in T1>(List<T> list, ObservableListChangeType changeType, params T1[] items);

        public event ObservableListChangedHandler<T> OnListChanged;

        public void Add(T item)
        {
            list.Add(item);
            OnListChanged?.Invoke(list, ObservableListChangeType.Add, item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            var enumerable = collection.ToList();
            list.AddRange(enumerable);
            OnListChanged?.Invoke(list, ObservableListChangeType.Add, enumerable.ToArray());
        }

        public bool Remove(T item)
        {
            bool result = list.Remove(item);
            if (result)
                OnListChanged?.Invoke(list, ObservableListChangeType.Remove, item);
            return result;
        }

        public void Clear()
        {
            list.Clear();
            OnListChanged?.Invoke(list, ObservableListChangeType.Clear);
        }

        public IReadOnlyList<T> List => list;
        public int Count => list.Count;
        public T this[int i] => list[i];
    }

    public enum ObservableListChangeType
    {
        Add,
        Remove,
        Clear
    }
}