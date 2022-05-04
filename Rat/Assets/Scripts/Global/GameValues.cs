using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Rat.UIRatHome;

public class GameValues
{
    public static readonly Color defaultFg = new Color(36 / 255f, 22 / 255f, 12 / 255f, 1f);
    public static readonly Color defaultBg = new Color(143 / 255f, 97 / 255f, 61 / 255f, 1f);
    public static readonly string defaultTag = "rat";
    public const float volumeMusicDefault = 0.7f;
    public const float volumeSFXDefault = 1f;

    private bool isMuted = false;
    public bool IsMuted
    {
        get
        {
            return isMuted;
        }
        set
        {
            isMuted = value;
            Rat.GameEvents.Instance.OnChangeMuteStatus?.Invoke();
        }
    }

    public bool hatEnabled = false;
    public int bestSoFar = 0;
    public Vector3 menuRatPosition;
    public Vector3 menuRatRotation;
    public Vector3 menuRatScale;

    ///////////////////////////////////////////////////////////////////////////////////////
    // User data. Please fire OnSavedUserDataUpdate when you change any of these below.
    public string savedTag = null;
    public Color foreground = defaultFg;
    public Color background = defaultBg;
    ///////////////////////////////////////////////////////////////////////////////////////

    private static GameValues _instance = null;
    public static GameValues Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameValues();

            return _instance;
        }
    }
}
