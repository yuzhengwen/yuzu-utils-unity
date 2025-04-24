using System;
using UnityEngine;

namespace YuzuValen.Utils
{
    public class CustomTick : MonoBehaviour
    {
        public static Action<float> OnCustomTick;
        private float tickTime = 0.2f;
        private float tickTimer = 0;

        void Update()
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= tickTime)
            {
                tickTimer = 0;
                OnCustomTick?.Invoke(tickTime);
            }
        }
    }
}