using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rat
{
    public class UILerpOnFinish : RatMonoBehaviour
    {
        [SerializeField] private Vector3 targetPos;
        [SerializeField] private Vector3 targetScale = Vector3.one;
        [SerializeField] private float duration = 1;

        private Vector3 startPos;
        private Vector3 startScale;
        private bool lerpEnabled = false;
        private float timeSoFar;

        private void Awake()
        {
            Events.OnCountdownEnd += () =>
            {
                if(this != null && this.gameObject != null)
                {
                    startPos = this.transform.localPosition;
                    startScale = this.transform.localScale;
                    lerpEnabled = true;
                }
            };
        }

        void Update()
        {
            if(lerpEnabled && timeSoFar < duration)
            {
                timeSoFar += Time.deltaTime;
                if(timeSoFar > duration)
                {
                    timeSoFar = duration;
                }

                float t = timeSoFar / duration;
                this.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
                this.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            }
        }
    }
}