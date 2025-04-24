using System;
using System.Collections.Generic;
using UnityEngine;

namespace YuzuValen.Utils.RPGStats
{
    [System.Serializable]
    public class StatModifier
    {
        public System.Object source;
        public float value;
        public StatModifierType type;

        /// <summary>
        /// Higher priority modifiers are applied later <br/>
        /// Be careful with this as it can lead to unexpected results <br/>
        /// Mostly used for override modifiers <br/>
        /// Default is 0
        /// </summary>
        public int priority = 0;

        public StatModifier(float value, StatModifierType type, System.Object source = null)
        {
            this.source = source;
            this.value = value;
            this.type = type;
        }

        #region overloads

        public static bool operator ==(StatModifier a, StatModifier b) =>
            a.source == b.source && a.type == b.type && Mathf.Approximately(a.value, b.value);

        public static bool operator !=(StatModifier a, StatModifier b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj is StatModifier mod)
            {
                return this == mod;
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(source, value, type);

        #endregion
    }

    public enum StatModifierType
    {
        /// <summary>
        /// Flat modifiers are added to the base value <br/>
        /// E.g. flat modifier 5 will add 5 to base value
        /// </summary>
        Flat,

        /// <summary>
        /// PercentAdd modifiers are added to the base value as a percentage <br/>
        /// E.g. percent add modifier 5% will add 5% of the base value to the base value
        /// </summary>
        PercentAdd,

        /// <summary>
        /// PercentMult modifiers are multiplied to the base value as a percentage <br/>
        /// E.g. percent mult modifier 105% will multiply the base value by 1.05
        /// </summary>
        PercentMult,

        /// <summary>
        /// Override modifiers replace the base value <br/>
        /// E.g. override modifier 5 will replace the base value with 5
        /// </summary>
        Override
    }

    public static class StatModifierApplicationExtensions
    {
        public static float ApplyModifiersWithPriority(this float baseValue, List<StatModifier> mods)
        {
            // sort based on priority (ascending)
            mods.Sort((a, b) => a.priority.CompareTo(b.priority));
            // group by priority (Every list contains modifiers with the same priority) (ascending)
            List<List<StatModifier>> pList = new();
            for (int i = 0; i < mods.Count; i++)
            {
                if (i == 0 || mods[i].priority != mods[i - 1].priority)
                    pList.Add(new List<StatModifier>());
                pList[^1].Add(mods[i]);
            }
            foreach (var pMods in pList)
            {
                baseValue = ApplyModifiers(baseValue, pMods);
            }

            return baseValue;
        }
        public static float ApplyModifiers(this float baseValue, List<StatModifier> mods)
        {
            float flatMods = 0;
            float percentAddMods = 0;
            float percentMultMods = 1;
            foreach (var mod in mods)
            {
                switch (mod.type)
                {
                    case StatModifierType.Flat:
                        flatMods += mod.value;
                        break;
                    case StatModifierType.PercentAdd:
                        percentAddMods += baseValue * (mod.value / 100);
                        break;
                    case StatModifierType.PercentMult:
                        percentMultMods *= mod.value / 100;
                        break;
                    case StatModifierType.Override:
                        return mod.value;
                }
            }

            return (baseValue + flatMods + percentAddMods) * percentMultMods;
        }
    }
}