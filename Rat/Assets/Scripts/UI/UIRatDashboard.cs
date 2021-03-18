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
        [SerializeField] private TextMeshProUGUI countdownText;

        void Awake()
        {
            // Event Dispatching
            ratButton.onClick.AddListener(() => Events.OnRatButton?.Invoke());
            ratButton.onClick.AddListener(() => Events.OnCountdownStart?.Invoke());

            // Event Listening
            Events.OnScoreChange += (score) => scoreText.text = score.ToString();
            Events.OnCountdownChange += (count) => countdownText.text = Mathf.Round(count).ToString();
            Events.OnCountdownEnd += () => ratButton.interactable = false;
        }
    }
}