using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rat
{
    public class RatGameplayVisual : RatMonoBehaviour
    {
        [SerializeField] private Vector3 rotateAdjustment = new Vector3(0, 0.1f, 0);
        [SerializeField] private float randMax = 2;
        [SerializeField] private GameObject hat;

        private void Awake()
        {
            Events.OnRatButton += () =>
            {
                rotateAdjustment.x = Random.Range(-randMax, randMax);
                rotateAdjustment.y = Random.Range(-randMax, randMax);
                rotateAdjustment.z = Random.Range(-randMax, randMax);
            };

            Events.OnRatButton += () =>
            {
                Events.OnPlaySound?.Invoke(AudioLabel.SoundRatClick, AudioType.SFX);
            };
        }

        private void Start()
        {
            hat.gameObject.SetActive(GameValues.Instance.hatEnabled);
        }

        void Update()
        {
            this.transform.Rotate(
                rotateAdjustment.x * Time.deltaTime,
                rotateAdjustment.y * Time.deltaTime,
                rotateAdjustment.z * Time.deltaTime, Space.Self);
        }
    }
}