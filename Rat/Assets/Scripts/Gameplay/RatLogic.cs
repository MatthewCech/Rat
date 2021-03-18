using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rat
{
    /// <summary>
    /// Main logic loop
    /// </summary>
    public class RatLogic : RatMonoBehaviour
    {
        [Header("Values")]
        [SerializeField] private int score = 0;
        [SerializeField] private float countdownDefault = 30;
        [SerializeField] private float countdownCurrent = 0;
        [SerializeField] private bool isCounting = false;
        [Header("Links")]
        [SerializeField] private GameObject rat;
        [SerializeField] private float ratExitSpeed = 1;
        [SerializeField] private string exitScene = "";
        [SerializeField] private List<GameObject> showWhenDone;

        private bool loadingScene = false;
        private void Awake()
        {
            Events.OnRatButton += IncrementScore;
            Events.OnCountdownEnd += OnEnd;
            Events.OnCountdownReset += () => { countdownCurrent = countdownDefault; };
            Events.OnCountdownStart += BeginCountdown;
            Events.OnLeaderboardEntryRequest += ScoreCreated;

            foreach (GameObject go in showWhenDone)
            {
                go.SetActive(false);
            }
        }

        private void ScoreCreated()
        {
            string tag = Globals.savedTag;

            if (tag == null)
            {
                tag = GameValues.defaultTag;
            }

            StartCoroutine(CreateScore(tag, Globals.foreground, Globals.background));
        }

        private IEnumerator CreateScore(string tag, Color fg, Color bg)
        {
            yield return LeaderboardUtil.SendScore(LeaderboardUtil.ConstructScore(tag, fg, bg, score),
                (res)=> {
                    Debug.Log($"Got back confirmation: {res}");
                    Done();
                },
                (err)=>
                {
                    Debug.LogError(err);
                    Done();
                });
        }

        private void Done()
        {
            if (!loadingScene)
            {
                loadingScene = true;
                SceneManager.LoadScene(exitScene);
            }
        }

        private void BeginCountdown()
        {
            isCounting = true;
            Events.OnHideIndicators?.Invoke();
        }

        private void Start()
        {
            countdownCurrent = countdownDefault;
            Events.OnCountdownStartup?.Invoke(countdownDefault);
            Events.OnScoreChange?.Invoke(score);

            Events.OnPlaySound?.Invoke(AudioLabel.MusicEnergy, AudioType.Music);
        }

        private void Update()
        {
            if (isCounting)
            {
                countdownCurrent -= Time.deltaTime;

                if (countdownCurrent < 0)
                {
                    countdownCurrent = 0;
                    Events.OnCountdownEnd?.Invoke();
                }

                Events.OnCountdownChange?.Invoke(countdownCurrent);
            }   
        }

        void IncrementScore()
        {
            ++score;
            Events.OnScoreChange?.Invoke(score);
        }

        void OnEnd()
        {
            foreach(GameObject go in showWhenDone)
            {
                go.SetActive(true);
            }

            StartCoroutine(RemoveRatLeft());
        }

        private IEnumerator RemoveRatLeft()
        {
            float timeLeft = 6;
            while(timeLeft > 0)
            {
                Vector3 pos = rat.transform.position;
                rat.transform.position = new Vector3(pos.x - ratExitSpeed * Time.deltaTime, pos.y, pos.z);
                timeLeft -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            rat.gameObject.SetActive(false);
        }
    }
}