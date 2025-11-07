using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YuzuValen.Utils.Stats
{
    /// <summary>
    /// A dynamic integer stat that has a effective max value and a current value
    /// </summary>
    [System.Serializable]
    public class IntStatDynamic : Stat
    {
        public int min = 0;
        public int currentValue;
        public float Percentage => currentValue / Value;

        public event Action<int> OnCurrentValueIncrease, OnCurrentValueDecrease, OnCurrentValueChanged;
        public event Action OnMinReached, OnMinReachedDelayed;

        public int IntValue => Mathf.FloorToInt(Value);
        public IntStatDynamic() { }
        public IntStatDynamic(int min, int baseMax, int currentValue)
        {
            this.min = min;
            this.BaseValue = baseMax;
            this.currentValue = currentValue;
        }

        public void Add(int amount)
        {
            Debug.Log($"Adding {amount} to {currentValue}");
            currentValue = Mathf.Clamp(currentValue + amount, min, IntValue);
            OnCurrentValueIncrease?.Invoke(amount);

            OnCurrentValueChanged?.Invoke(currentValue);
        }
        public void Remove(int amount)
        {
            currentValue = Mathf.Clamp(currentValue - amount, min, IntValue);
            OnCurrentValueDecrease?.Invoke(amount);
            if (currentValue <= min)
            {
                OnMinReached?.Invoke();
                OnMinReachedDelayed?.Invoke();
            }

            OnCurrentValueChanged?.Invoke(currentValue);
        }

        public void SetCurrentToMax() => Add(IntValue - currentValue);
        public bool IsMax() => currentValue == Value;
        public void CopyValues(IntStatDynamic stat, bool retainCurrentValue = false)
        {
            statModifiers = new List<StatModifier>(stat.statModifiers);
            min = stat.min;
            BaseValue = stat.BaseValue; // this will call UpdateValue()
            if (!retainCurrentValue)
                currentValue = stat.currentValue;
            // in case max/min changes and current value is out of bounds
            Mathf.Clamp(currentValue, min, Value);
            OnCurrentValueChanged?.Invoke(currentValue);
        }
    }
}
