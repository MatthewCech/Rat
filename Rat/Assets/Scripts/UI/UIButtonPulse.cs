using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rat
{
    public class UIButtonPulse : MonoBehaviour
    {
        // Inspector Variables
        [SerializeField] private float amplitude = 0.1f;
        [SerializeField] private float rate = 1f;

        // Private Variables
        private Vector3 startingScale;

        private void Start()
        {
            startingScale = transform.localScale;
        }

        void Update()
        {
            transform.localScale = startingScale * (1 + Mathf.Sin(Time.timeSinceLevelLoad * rate) * amplitude);
        }
    }
}