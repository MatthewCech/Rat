using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rat
{
    public class GameEvents
    {
          ////////////////////////
         // Events and Actions //
        ////////////////////////    
        // UI
        public Action OnRatButton;
        public Action<string> OnSlideIn;
        public Action OnHideIndicators;
        public Action OnStartButtonHit;
        public Action OnSavedUserDataUpdate;

        // Countdown
        public Action OnCountdownStart;
        public Action OnCountdownEnd;
        public Action<float> OnCountdownChange;
        public Action OnCountdownReset;
        public Action<float> OnCountdownStartup;

        // Score
        public Action<int> OnScoreChange;
        public Action OnScoreReset;
        public Action OnLeaderboardEntryRequest;

        // Audio
        public Action<AudioLabel, AudioType> OnPlaySound;

          //////////////////////
         // Event management //
        //////////////////////

        private static GameEvents _instance = null;
        public static GameEvents Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameEvents();

                return _instance;
            }
        }

    }
}