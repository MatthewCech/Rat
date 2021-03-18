using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadingText : MonoBehaviour
{
    [SerializeField] private float dotDelay = 0.5f;
    [SerializeField] private TMPro.TextMeshProUGUI loadingText;

    private float timeSoFar;
    private int numDots;
    private string startText;

    private void Start()
    {
        startText = loadingText.text;
    }

    private void Update()
    {
        timeSoFar += Time.deltaTime;
        if(timeSoFar > dotDelay)
        {
            timeSoFar = 0;
            if(++numDots > 3)
            {
                numDots = 0;
            }

            loadingText.text = startText + new System.String('.', numDots);
        }
    }
}
