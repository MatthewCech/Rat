using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Rat
{
    [RequireComponent(typeof(Image))]
    public class UIHandIndicate : RatMonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private float amplitude = 1;
        [SerializeField] private float startAlpha = -1;
        [SerializeField] private float fadeTime = 3;
        [SerializeField] private float offset = 0;

        private Vector3 startPos;
        private Image image;
        private float timeSoFar;
        private void Awake()
        {
            startPos = this.transform.localPosition;
            image = this.GetComponent<UnityEngine.UI.Image>();
            Color color = image.color;
            color.a = startAlpha;
            image.color = color;
        }

        private void OnEnable()
        {
            Events.OnHideIndicators += OnHide;
        }

        private void OnDisable()
        {
            Events.OnHideIndicators -= OnHide;
        }

        private void OnHide()
        {
            this.gameObject.SetActive(false);
        }

        private void Update()
        {
            float xOffset = Mathf.Sin(Time.realtimeSinceStartup * speed + offset) * amplitude;
            transform.localPosition = new Vector3(startPos.x + xOffset, startPos.y, startPos.z);

            if (timeSoFar < fadeTime)
            {
                timeSoFar += Time.deltaTime;
                if(timeSoFar > fadeTime)
                {
                    timeSoFar = fadeTime;
                }

                float t = timeSoFar / fadeTime;

                Color color = image.color;
                color.a = Mathf.Lerp(startAlpha, 1, t);
                image.color = color;
            }
        }
    }
}