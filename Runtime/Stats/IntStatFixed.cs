using System.Collections.Generic;
using UnityEngine;

namespace YuzuValen.Utils.Stats
{
    /// <summary>
    /// A fixed integer stat that has only one effective value
    /// </summary>
    [System.Serializable]
    public class IntStatFixed: Stat
    {
        public int IntValue => Mathf.FloorToInt(Value);
        public IntStatFixed() { }
        public IntStatFixed(int baseValue)
        {
            this.BaseValue = baseValue;
        }

        public void CopyValue(IntStatFixed stat)
        {
            statModifiers = new List<StatModifier>(stat.statModifiers);
            BaseValue = stat.BaseValue; // this will trigger UpdateValue()
        }
    }
}
