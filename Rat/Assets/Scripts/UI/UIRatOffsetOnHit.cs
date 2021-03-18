using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rat
{
    public class UIRatOffsetOnHit : RatMonoBehaviour
    {
        [SerializeField] private Vector3 startRot;
        [SerializeField] private float duration = 0.1f;

        private float timeSoFar;
        private Vector3 targetRot;
        private bool canLerp;
        private void Awake()
        {
            targetRot = this.transform.rotation.eulerAngles;
            this.transform.rotation = Quaternion.Euler(startRot);
            Events.OnStartButtonHit += () => { canLerp = true; };
        }

        private void Update()
        {
            if(canLerp && timeSoFar < duration)
            {
                timeSoFar += Time.deltaTime;
                if(timeSoFar > duration)
                {
                    timeSoFar = duration;
                }

                float t = timeSoFar / duration;
                this.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRot, targetRot, t));
            }
        }
    }
}
