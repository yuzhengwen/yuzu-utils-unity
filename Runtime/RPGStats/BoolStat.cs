using UnityEngine;

namespace YuzuValen.Utils.RPGStats
{
    [System.Serializable]
    public class BoolStat : Stat
    {
        public bool BoolValue => Value > 0; // 0 is false, 1 is true
        public BoolStat() { }
        public BoolStat(bool baseValue)
        {
            this.BaseValue = baseValue ? 1 : 0;
        }

        public override void AddModifier(StatModifier mod)
        {
            if (mod.type != StatModifierType.Override)
            {
                Debug.LogWarning("BoolStatFixed does not support non-override modifiers");
                return;
            }
            base.AddModifier(mod);
        }

        public override bool RemoveModifier(StatModifier mod)
        {
            if (mod.type != StatModifierType.Override)
            {
                Debug.LogWarning("BoolStatFixed does not support non-override modifiers");
                return false;
            }
            return base.RemoveModifier(mod);
        }
    }
}