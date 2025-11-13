using UnityEngine;

namespace YuzuValen.Utils
{
    public static class MonoBehaviourExtensions
    {
        public static T GetComponentInParentsRecursive<T>(this MonoBehaviour obj) 
        {
            if (obj.transform.parent != null)
            {
                return obj.transform.parent.GetComponentInParent<T>();
            }

            return default;
        }
    }
}