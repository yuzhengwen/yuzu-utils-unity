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
    }
}