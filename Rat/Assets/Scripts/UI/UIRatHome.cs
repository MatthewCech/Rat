using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rat
{
    public class UIRatHome : RatMonoBehaviour
    {
        public UILeaderboardEntry entryTemplate;
        public UILoadingText loadingText;
        public TMPro.TextMeshProUGUI errorText;

        private void Start()
        {
            Events.OnPlaySound?.Invoke(AudioLabel.MusicChill, AudioType.Music);

            errorText.gameObject.SetActive(false);
            entryTemplate.gameObject.SetActive(false);
            StartCoroutine(LoadLeaderboard());
        }

        private IEnumerator LoadLeaderboard()
        {
            // Make web request and wait for result
            yield return null;
            yield return LeaderboardUtil.GetRawLeaderboard(
                (res) => {
                    Debug.Log("Got a result");
                    StartCoroutine(ParseLeaderboard(res));
                },
                (err) => {
                    Debug.LogError(err);
                    loadingText.gameObject.SetActive(false);
                    errorText.gameObject.SetActive(true);
                    errorText.text = err;
                });
        }

        private IEnumerator ParseLeaderboard(string raw)
        {
            // Try to parse json
            yield return null;
            Debug.Log("Trying to parse leaderboard");
            LeaderboardUtil.Leaderboard leaderboard = JsonUtility.FromJson<LeaderboardUtil.Leaderboard>(raw);

            // Perform sorting.
            yield return null;
            Debug.Log("Verifying leaderboard sorting");
            yield return StartCoroutine(LeaderboardUtil.SortLeaderboard(leaderboard));

            // Construt loading items with delay
            yield return null;
            Debug.Log("Adding entries");
            const float delay = 0.025f;
            for (int i = 0; i < leaderboard.entries.Count; ++i)
            {
                LeaderboardUtil.ScoreEntry score = leaderboard.entries[i];

                UILeaderboardEntry entry = GameObject.Instantiate(entryTemplate, entryTemplate.transform.parent);
                Color fg = LeaderboardUtil.ScoreEntry.ColorFromString(score.fg);
                Color bg = LeaderboardUtil.ScoreEntry.ColorFromString(score.bg);
                entry.SetValue(score.tag, score.score.ToString(), i + 1, fg, bg);
                entry.gameObject.SetActive(true);
                yield return new WaitForSeconds(delay);
            }

            // Turn off loading text
            yield return null;
            Debug.Log("Done");
            loadingText.gameObject.SetActive(false);
        }
    }
}