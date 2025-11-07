using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace YuzuValen.Utils.Stats
{
    [System.Serializable]
    public class StatModifier
    {
        public System.Object source;
        public float value;
        public StatModifierType type;

        public StatModifier(float value, StatModifierType type, System.Object source = null)
        {
            this.source = source;
            this.value = value;
            this.type = type;
        }

        #region overloads

        public static bool operator ==(StatModifier a, StatModifier b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.source == b.source && a.type == b.type && Mathf.Approximately(a.value, b.value);
        }

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

        public string GetDescription()
        {
            return type switch
            {
                StatModifierType.Flat => $"+{value} (Flat)",
                StatModifierType.PercentAdd => $"+{value}% (Additive Multiplier)",
                StatModifierType.PercentMult => $"*{value}% (Multiplicative)",
                _ => value.ToString(CultureInfo.InvariantCulture)
            };
        }
    }

    public enum StatModifierType
    {
        Flat,
        PercentAdd,
        PercentMult
    }
}