using System.Collections.Generic;
using UnityEngine;

namespace YuzuValen
{
    [CreateAssetMenu(fileName = "ScriptableObjectReferenceHolder", menuName = "ScriptableObject/Reference Holder")]
    public class ScriptableObjectReferenceHolder : ScriptableObject
    {
        public List<ScriptableObject> references;
    
        public int GetIndex(ScriptableObject obj)
        {
            return references.IndexOf(obj);
        }

        public ScriptableObject GetReference(int index)
        {
            if (index >= 0 && index < references.Count)
                return references[index];
            return null;
        }
    }
}