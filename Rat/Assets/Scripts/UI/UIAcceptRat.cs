using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rat
{
    [RequireComponent(typeof(Button))]
    public class UIAcceptRat : RatMonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI pending;

        private Button target;
        private bool clicked = false;

        private void Awake()
        {
            target = this.GetComponent<UnityEngine.UI.Button>();
            Events.OnCountdownEnd += OnCoutdownEnd;
            target.onClick.AddListener(OnClick);
            pending.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            target.onClick.RemoveListener(OnClick);
            Events.OnCountdownEnd -= OnCoutdownEnd;
        }

        private void OnCoutdownEnd()
        {
            this.gameObject.SetActive(true);
        }

        private void OnClick()
        {
            if (!clicked)
            {
                clicked = true;
                pending.gameObject.SetActive(true);
                Events.OnLeaderboardEntryRequest?.Invoke();
            }
        }
    }
}