using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rat
{
    public class UILerpOnFinish : RatMonoBehaviour
    {
        [SerializeField] private Vector3 scale = Vector3.one;
        [SerializeField] private float duration = .5f;
        [SerializeField] private bool lerpsToScaleInsteadOfFrom = true;
        [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Vector3 startScale;
        private bool lerpEnabled = false;
        private float timeSoFar;

        private void Awake()
        {
            startScale = this.transform.localScale;

            if(!lerpsToScaleInsteadOfFrom)
            {
                this.transform.localScale = scale;
            }

            Events.OnCountdownEnd += () =>
            {
                if(this != null && this.gameObject != null)
                {
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

                float rawT = timeSoFar / duration;
                float t = curve.Evaluate(rawT);

                if (lerpsToScaleInsteadOfFrom)
                {
                    this.transform.localScale = Vector3.Lerp(startScale, scale, t);
                }
                else
                {
                    this.transform.localScale = Vector3.Lerp(scale, startScale, t);
                }
            }
        }
    }
}