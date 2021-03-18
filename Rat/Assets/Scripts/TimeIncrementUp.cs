using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rat
{
    public class TimeIncrementUp : RatMonoBehaviour
    {
        [SerializeField] private string identifier;
        [SerializeField] private float timeBetweenTicks = 0.05f;
        
        private bool countUp;
        private float currentTime;
        private float targetValue;
        private float currentValue;

        private void Awake()
        {
            Events.OnSlideIn += (string id) =>
            {
                if (identifier.ToLowerInvariant().Equals(id.ToLowerInvariant()))
                {
                    countUp = true;
                }
            };

            Events.OnCountdownStartup += (count) =>
            {
                targetValue = count;
                Events.OnCountdownChange?.Invoke(0);
            };

            currentValue = 0;
        }

        private void Update()
        {
            if(countUp)
            {
                currentTime += Time.deltaTime;

                if(currentTime > timeBetweenTicks)
                {
                    if (currentValue < targetValue)
                    {
                        currentValue += 1;
                    }
                    else
                    {
                        currentValue = targetValue;
                        countUp = false;
                    }

                    Events.OnCountdownChange?.Invoke(currentValue);
                }
            }
        }
    }
}