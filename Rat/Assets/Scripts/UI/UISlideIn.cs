using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rat
{
    public class UISlideIn : RatMonoBehaviour
    {
        enum SlideState
        {
            Delaying,
            Lerping,
            Done
        };

        [SerializeField] private float startingOffset = 10;
        [SerializeField] private bool invertDirection = false;
        [SerializeField] private float time = 1; // seconds
        [SerializeField] private float delay = 1; // seconds
        [SerializeField] private string identifier = "";

        private float timeCurrent = 0;
        private Vector3 finalPosition;
        private Vector3 startPosition;
        private SlideState state = SlideState.Delaying;

        // Start is called before the first frame update
        void Awake()
        {
            finalPosition = transform.position;

            if (invertDirection)
                startingOffset *= -1;

            startPosition = new Vector3(
                transform.position.x, 
                transform.position.y + startingOffset, 
                transform.position.z);

            this.transform.position = this.startPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if(state == SlideState.Delaying)
            {
                delay -= Time.deltaTime;

                if (delay < 0)
                    state = SlideState.Lerping;
            }

            if (state == SlideState.Lerping)
            {
                if (timeCurrent < 0)
                    timeCurrent = 0;

                float t = timeCurrent / time;

                if (timeCurrent < time)
                {
                    timeCurrent += Time.deltaTime;
                }
                else
                {
                    t = 1;
                    timeCurrent = time;
                    state = SlideState.Done;
                    Events.OnSlideIn?.Invoke(identifier);
                }

                if (t > 1)
                    t = 1;

                transform.position = Vector3.Lerp(startPosition, finalPosition, t);
            }
        }
    }
}