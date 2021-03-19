using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class LeaderboardUtil
{
    public static int CompareScores(ScoreEntry lhs, ScoreEntry rhs)
    {
        return rhs.score - lhs.score;
    }

    // Orders as largest to smallest
    public static IEnumerator SortLeaderboard(Leaderboard leaderboard)
    {
        yield return null;
        
        // Hand-imlpement if this gets to be too time consuming
        leaderboard.entries.Sort(CompareScores);

        yield return null;
    }

    public static ScoreEntry ConstructScore(string tag, Color fg, Color bg, int scoreVal)
    {
        ScoreEntry score = new ScoreEntry();
        score.tag = tag;
        score.fg = $"{Mathf.RoundToInt(fg.r * 255)}.{Mathf.RoundToInt(fg.g * 255)}.{Mathf.RoundToInt(fg.b * 255)}";
        score.bg = $"{Mathf.RoundToInt(bg.r * 255)}.{Mathf.RoundToInt(bg.g * 255)}.{Mathf.RoundToInt(bg.b * 255)}";
        score.score = scoreVal;

        return score;
    }

    public const string endpoint = "https://rw0.pw/api/v1/rat";

    public static IEnumerator SendScore(ScoreEntry entry, System.Action<string> onComplete, System.Action<string> onError)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get($"{endpoint}/add?tag={entry.tag}&fg={entry.fg}&bg={entry.bg}&score={entry.score}"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                onError?.Invoke(webRequest.responseCode.ToString());
            }
            else
            {
                onComplete?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }

    public static IEnumerator GetRawLeaderboard(System.Action<string> onComplete, System.Action<string> onError)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get($"{endpoint}/dump"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                onError?.Invoke(webRequest.responseCode.ToString());
            }
            else
            {
                onComplete?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }

    [System.Serializable]
    public class Leaderboard
    {
        public List<ScoreEntry> entries;
    }

    [System.Serializable]
    public class ScoreEntry
    {
        public string tag;
        public string fg;
        public string bg;

        public int score;

        // Parses a string in the proprietary format of r.g.b, where each value
        // is anywhere from 0 and 255, inclusive.
        public static Color ColorFromString(string input)
        {
            Color c = new Color();
            c.a = 1;

            try
            {
                string[] vals = input.Split('.');
                c.r = Mathf.Clamp(int.Parse(vals[0]), 0, 255) / 255f;
                c.g = Mathf.Clamp(int.Parse(vals[1]), 0, 255) / 255f;
                c.b = Mathf.Clamp(int.Parse(vals[2]), 0, 255) / 255f;
            }
            catch (System.Exception)
            {
                Debug.LogError($"Error parsing dot delimited solid color from string: {input}");
                return c;
            }

            return c;
        }
    }
}
