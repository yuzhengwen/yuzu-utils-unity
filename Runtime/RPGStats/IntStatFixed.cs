using UnityEngine;

namespace YuzuValen.Utils.RPGStats
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
    }
    [System.Serializable]
    public class FloatStatFixed : Stat
    {
        public FloatStatFixed() { }
        public FloatStatFixed(float baseValue)
        {
            this.BaseValue = baseValue;
        }
    }
}
