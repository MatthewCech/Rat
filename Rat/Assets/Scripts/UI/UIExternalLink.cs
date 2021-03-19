using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rat
{
    [RequireComponent(typeof(Button))]
    public class UIExternalLink : RatMonoBehaviour
    {
        [SerializeField] private string targetURL;

        private void OnEnable()
        {
            this.GetComponent<Button>().onClick.AddListener(OpenTarget);
        }

        private void OnDisable()
        {
            this.GetComponent<Button>().onClick.RemoveListener(OpenTarget);
        }

        private void OpenTarget()
        {
            Application.OpenURL(targetURL);
        }
    }
}