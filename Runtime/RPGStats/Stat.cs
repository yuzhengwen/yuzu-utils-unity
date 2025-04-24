using System.Collections.Generic;
using UnityEngine;

namespace YuzuValen.Utils.RPGStats
{
    /// <summary>
    /// Uses floats for all values to allow for decimal precision
    /// </summary>
    public abstract class Stat
    {
        private bool isDirty = true;
        [SerializeField] private float baseValue;

        /// <summary>
        /// Base value of the stat
        /// </summary>
        public float BaseValue
        {
            get => baseValue;
            set
            {
                baseValue = value;
                isDirty = true;
            }
        }

        [SerializeField] [ReadOnlyInspector] public List<StatModifier> statModifiers = new();

        [SerializeField] [ReadOnlyInspector] private float value;

        /// <summary>
        /// Final value of the stat after applying all modifiers
        /// Cannot be set directly
        /// </summary>
        public float Value
        {
            get
            {
                if (isDirty)
                {
                    UpdateValue();
                    isDirty = false;
                }

                return value;
            }
            protected set => this.value = value;
        }

        public virtual void AddModifier(StatModifier mod)
        {
            statModifiers.Add(mod);
            isDirty = true;
        }

        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }

            return false;
        }

        // NOTE: Update manually to avoid doing redundant LINQ operations when value has not changed
        public virtual void UpdateValue()
        {
            Value = Mathf.FloorToInt(BaseValue.ApplyModifiersWithPriority(statModifiers));
            /*
            var flatMods = statModifiers.FindAll(mod => mod.type == StatModifierType.Flat).Sum(mod => mod.value);
            var percentAddMods = statModifiers.FindAll(mod => mod.type == StatModifierType.PercentAdd).Aggregate(0f, (acc, mod) => acc + BaseValue * (mod.value / 100));
            var percentMultMods = statModifiers.FindAll(mod => mod.type == StatModifierType.PercentMult).Aggregate(1f, (acc, mod) => acc * (1 + mod.value / 100));
            Value = Mathf.FloorToInt((BaseValue + flatMods + percentAddMods) * percentMultMods);*/
        }

        public void ClearModifiers()
        {
            statModifiers.Clear();
            isDirty = true;
        }

        public bool RemoveModifiersOfType(StatModifierType type)
        {
            var res = statModifiers.RemoveAll(mod => mod.type == type) > 0;
            isDirty = true;
            return res;
        }

        public bool RemoveModifiersFrom(System.Object source)
        {
            var res = statModifiers.RemoveAll(mod => mod.source == source) > 0;
            isDirty = true;
            return res;
        }

        public List<StatModifier> GetAllModifiersOfType(StatModifierType type) =>
            statModifiers.FindAll(mod => mod.type == type);

        public List<StatModifier> GetAllModifiersFrom(System.Object source) =>
            statModifiers.FindAll(mod => mod.source == source);

        public virtual void CopyValue(Stat stat)
        {
            statModifiers = new List<StatModifier>(stat.statModifiers);
            BaseValue = stat.BaseValue; // this will trigger UpdateValue()
        }
    }
}