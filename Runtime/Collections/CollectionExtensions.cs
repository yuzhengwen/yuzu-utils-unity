using System;
using System.Collections.Generic;

namespace YuzuValen.Utils.Collections
{
    public static class CollectionExtensions
    {
        public static T GetRandomElement<T>(this IList<T> list)
        {
            if (list.Count == 0) return default;
            var rand = new System.Random();
            var index = rand.Next(0, list.Count);
            return list[index];
        }

        public static T GetRandomValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            var rand = new System.Random();
            int index = rand.Next(0, values.Length);
            return (T)values.GetValue(index);
        }

        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                (ts[i], ts[r]) = (ts[r], ts[i]);
            }
        }
    }
}