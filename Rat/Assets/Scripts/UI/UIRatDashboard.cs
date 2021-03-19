using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Rat
{
    public class UIRatDashboard : RatMonoBehaviour
    {
        [SerializeField] private Button ratButton;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI scoreTextAlt;
        [SerializeField] private TextMeshProUGUI countdownText;

        void OnEnable()
        {
            // Event Dispatching
            ratButton.onClick.AddListener(() => Events.OnRatButton?.Invoke());
            ratButton.onClick.AddListener(() => Events.OnCountdownStart?.Invoke());

            // Event Listening
            Events.OnScoreChange += UpdateScoreText;
            Events.OnCountdownChange += ShowCountdown;
            Events.OnCountdownEnd += DisableInteraction;
        }

        private void UpdateScoreText(int scoreInt)
        {
            string score = scoreInt.ToString();
            scoreText.text = score;
            scoreTextAlt.text = score;
        }

        private void DisableInteraction()
        {
            ratButton.interactable = false;
        }

        private void ShowCountdown(float count)
        {
            countdownText.text = Mathf.Round(count).ToString();
        }

        private void OnDisable()
        {
            Events.OnCountdownEnd -= DisableInteraction;
            Events.OnCountdownChange -= ShowCountdown;
            Events.OnScoreChange -= UpdateScoreText;

            ratButton.onClick.RemoveAllListeners();
        }
    }
}