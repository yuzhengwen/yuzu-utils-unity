using System.Collections.Generic;
using UnityEngine;

namespace YuzuValen.Utils
{
    public static class MiscExtensions
    {
        public static LayerMask RemoveLayer(this LayerMask layerMask, string layerName)
        {
            int layerToRemove = LayerMask.NameToLayer(layerName);
            int layerMaskValue = 1 << layerToRemove;
            return layerMask &= ~layerMaskValue;
        }

        public static LayerMask AddLayer(this LayerMask layerMask, string layerName)
        {
            int layerToAdd = LayerMask.NameToLayer(layerName);
            int layerMaskValue = 1 << layerToAdd;
            return layerMask |= layerMaskValue;
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