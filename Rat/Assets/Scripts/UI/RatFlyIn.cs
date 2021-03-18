using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rat
{
    public class RatFlyIn : RatMonoBehaviour
    {
        [SerializeField] private Vector3 startPos;
        [SerializeField] private Vector3 startRot;
        [SerializeField] private Vector3 startScale;
        [SerializeField] private float duration;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float startDelay = 0.5f;
        [Space]
        [SerializeField] private bool playSFX = true;
        [SerializeField] private bool dispatchEvent = true;
        [SerializeField] private bool saveRatPosition = true;
        [SerializeField] private bool useSavedPosition = false;

        // Rat
        private bool playingSFX = false;
        private float timeSoFar;
        private Vector3 targetPos;
        private Vector3 targetRot;
        private Vector3 targetScale;

        // Start is called before the first frame update
        void Start()
        {
            timeSoFar = -startDelay;

            // Rat
            if (useSavedPosition)
            {
                this.startPos = GameValues.Instance.menuRatPosition;
                this.startRot = GameValues.Instance.menuRatRotation;
                this.startScale = GameValues.Instance.menuRatScale;
            }

            this.targetPos = this.transform.position;
            this.targetRot = this.transform.rotation.eulerAngles;
            this.targetScale = this.transform.localScale;

            this.transform.position = this.startPos;
            this.transform.rotation = Quaternion.Euler(this.startRot);
            this.transform.localScale = this.startScale;
        }

        // Update is called once per frame
        void Update()
        {

            // Step 1: pos lerp
            if (timeSoFar < duration)
            {
                timeSoFar += Time.deltaTime;

                bool lastStep = false;
                if (timeSoFar >= duration)
                {
                    lastStep = true;
                    timeSoFar = duration;
                }

                float rawT = timeSoFar / duration;
                float t = curve.Evaluate(rawT);

                this.transform.position = Vector3.Lerp(startPos, targetPos, t);
                this.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRot, targetRot, t));
                this.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

                if (playSFX && playingSFX == false && timeSoFar > duration * .8f)
                {
                    playingSFX = true;
                    Events.OnPlaySound?.Invoke(AudioLabel.SoundRatClick, AudioType.SFX);
                }

                if (lastStep)
                {
                    if (dispatchEvent)
                    {
                        Events.OnStartButtonHit?.Invoke();
                    }

                    if (saveRatPosition)
                    {
                        GameValues.Instance.menuRatPosition = transform.position;
                        GameValues.Instance.menuRatRotation = transform.rotation.eulerAngles;
                        GameValues.Instance.menuRatScale = transform.localScale;
                    }
                }
            }
        }
    }
}