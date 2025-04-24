using System;
using UnityEngine;

namespace YuzuValen.Utils
{
    public class Timer
    {
        private float timer;

        public Action OnComplete;
        public float duration;
        public bool repeat;
        public bool paused = false;

        public Timer(float duration, bool repeat = false)
        {
            this.duration = duration;
            this.repeat = repeat;
        }
        public void Update()
        {
            if (paused) return;
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                OnComplete?.Invoke();
                if (repeat)
                {
                    timer = 0;
                }
                else
                    paused = true;
            }
        }
        public float GetCurrentTimeCount()
        {
            return timer;
        }
        public void Restart()
        {
            timer = 0;
            paused = false;
        }
    }
}
