using System;
using System.Collections.Generic;

namespace YuzuValen.Utils
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
    }
}