using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YuzuValen.Utils.Stats
{
    /// <summary>
    /// Uses floats for all values to allow for decimal precision
    /// </summary>
    [System.Serializable]
    public class Stat : ISerializationCallbackReceiver
    {
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
                UpdateValue();
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
            get => value;
            protected set => this.value = value;
        }

        public Stat()
        {
        }

        public Stat(float baseValue)
        {
            BaseValue = baseValue;
        }

        public virtual void AddModifier(StatModifier mod)
        {
            statModifiers.Add(mod);
            UpdateValue();
        }

        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                UpdateValue();
                return true;
            }

            return false;
        }

        // NOTE: Update manually to avoid doing redundant LINQ operations when value has not changed
        public virtual void UpdateValue()
        {
            var flatMods = statModifiers.FindAll(mod => mod.type == StatModifierType.Flat).Sum(mod => mod.value);
            var percentAddMods = statModifiers.FindAll(mod => mod.type == StatModifierType.PercentAdd)
                .Aggregate(0f, (acc, mod) => acc + BaseValue * (mod.value / 100));
            var percentMultMods = statModifiers.FindAll(mod => mod.type == StatModifierType.PercentMult)
                .Aggregate(1f, (acc, mod) => acc * (1 + mod.value / 100));
            Value = (BaseValue + flatMods + percentAddMods) * percentMultMods;
        }

        public void ClearModifiers()
        {
            statModifiers.Clear();
            UpdateValue();
        }

        public bool RemoveModifiersOfType(StatModifierType type)
        {
            var res = statModifiers.RemoveAll(mod => mod.type == type) > 0;
            UpdateValue();
            return res;
        }

        public bool RemoveModifiersFrom(System.Object source)
        {
            var res = statModifiers.RemoveAll(mod => mod.source == source) > 0;
            UpdateValue();
            return res;
        }

        public List<StatModifier> GetAllModifiersOfType(StatModifierType type) =>
            statModifiers.FindAll(mod => mod.type == type);

        public List<StatModifier> GetAllModifiersFrom(System.Object source) =>
            statModifiers.FindAll(mod => mod.source == source);

        public void OnBeforeSerialize()
        {
        }

        /// <summary>
        /// Allows 'value' to update after editing 'base value' in inspector
        /// </summary>
        public void OnAfterDeserialize()
        {
            UpdateValue();
        }
    }
}