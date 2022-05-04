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
        [SerializeField] private Button acceptScore;
        [SerializeField] private UILerpOnFinish lerp;
        [SerializeField] private TMPro.TextMeshProUGUI accidentalClickPreventText;

        private Button target;
        private bool clicked = false;
        private int submitCooldownSeconds = 3;

        private string startText;

        private void Awake()
        {
            startText = accidentalClickPreventText.text;

            target = this.GetComponent<UnityEngine.UI.Button>();
            Events.OnCountdownEnd += OnCoutdownEnd;
            target.onClick.AddListener(OnClick);
            pending.gameObject.SetActive(false);
            accidentalClickPreventText.gameObject.SetActive(false);

            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            target.onClick.RemoveListener(OnClick);
            Events.OnCountdownEnd -= OnCoutdownEnd;
        }

        private void OnCoutdownEnd()
        {
            acceptScore.interactable = false;
            accidentalClickPreventText.gameObject.SetActive(true);

            this.gameObject.SetActive(true);
            StartCoroutine(StartSubmitCooldown(() => { 
                acceptScore.interactable = true; 
                accidentalClickPreventText.gameObject.SetActive(false); 
            }));
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

        private IEnumerator StartSubmitCooldown(System.Action onComplete)
        {
            yield return new WaitForSeconds(lerp.Duration);

            while(submitCooldownSeconds > 0)
            {
                accidentalClickPreventText.text = $"{startText} {submitCooldownSeconds}";
                --submitCooldownSeconds;

                yield return new WaitForSeconds(1);
            }

            onComplete?.Invoke();
        }
    }
}