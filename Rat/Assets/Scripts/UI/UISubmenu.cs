using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rat
{
    public class UISubmenu : RatMonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private float duration;
        [SerializeField] Vector3 startOffset;
        [SerializeField] private AnimationCurve curve;

        private bool canDisplay = false;
        private float timeSoFar;
        private Vector3 targetPos;
        private void Start()
        {
            Events.OnStartButtonHit += () => { canDisplay = true; };
            targetPos = target.transform.localPosition;
            target.transform.localPosition = targetPos + startOffset;
        }

        private void Update()
        {
            if(canDisplay && timeSoFar < duration)
            {
                timeSoFar += Time.deltaTime;
                if(timeSoFar > duration)
                {
                    timeSoFar = duration;
                }

                float rawT = timeSoFar / duration;
                float t = curve.Evaluate(rawT);

                target.transform.localPosition = Vector3.Lerp(targetPos + startOffset, targetPos, t);
            }
        }
    }
}