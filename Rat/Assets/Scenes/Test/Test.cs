using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private LeaderboardUtil.ScoreEntry toGarble;

    public void Start()
    {
        string garbled = LeaderboardUtil.Garble(toGarble);
        Debug.Log($"Garbled: {garbled}");

        string ungarbled = LeaderboardUtil.UnGarble(garbled);
        Debug.Log($"Ungarbled: {ungarbled}");
    }
}
