using System;
using UnityEngine;

namespace YuzuValen.Utils.RPGStats
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

        public IntStatDynamic()
        {
        }

        public IntStatDynamic(int min, int baseMax, int currentValue)
        {
            this.min = min;
            this.BaseValue = baseMax;
            this.currentValue = currentValue;
        }

        public void Add(int amount)
        {
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

        public void CopyValue(IntStatDynamic stat, bool retainCurrentValue = false)
        {
            base.CopyValue(stat);
            min = stat.min;
            if (!retainCurrentValue)
                currentValue = stat.currentValue;
            // in case max/min changes and current value is out of bounds
            Mathf.Clamp(currentValue, min, Value);
            OnCurrentValueChanged?.Invoke(currentValue);
        }

        public override void CopyValue(Stat stat)
        {
            if (stat is IntStatDynamic intStat)
            {
                CopyValue(intStat);
                return;
            }

            Debug.LogError("Trying to copy from another stat implementation to IntStatDynamic");
        }
    }
}