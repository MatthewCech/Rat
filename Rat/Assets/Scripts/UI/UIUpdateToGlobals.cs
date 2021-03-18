using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rat
{
    public class UIUpdateToGlobals : RatMonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI tag;
        [SerializeField] private Image background;
        [SerializeField] private Button reset;

        private void OnEnable()
        {
            Events.OnSavedUserDataUpdate += Redraw;
            reset.onClick.AddListener(Reset);
        }

        private void OnDisable()
        {
            Events.OnSavedUserDataUpdate -= Redraw;
            reset.onClick.RemoveListener(Reset);
        }

        private void Reset()
        {
            Globals.savedTag = null;
            Globals.foreground = GameValues.defaultFg;
            Globals.background = GameValues.defaultBg;
            Events.OnSavedUserDataUpdate?.Invoke();
        }

        private void Redraw()
        {
            if (Globals.savedTag != null)
            {
                tag.text = Globals.savedTag;
            }
            else
            {
                tag.text = "rat";
            }

            tag.color = Globals.foreground;
            background.color = Globals.background;
        }
    }
}