using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rat
{
    public class UIColorBuilder : RatMonoBehaviour
    {
        enum TargetValue
        {
            foreground,
            background
        }

        [SerializeField] private TargetValue target;
        [SerializeField] private Button buttonTemplate;

        public readonly Color[] colors = new Color[]
        {
            new Color(0/255f, 0/255f, 0/255f, 1f),
            new Color(127/255f, 127/255f, 127/255f, 1f),
            new Color(136/255f, 0/255f, 21/255f, 1f),
            new Color(237/255f, 28/255f, 36/255f, 1f),
            new Color(255/255f, 127/255f, 39/255f, 1f),
            new Color(255/255f, 242/255f, 0/255f, 1f),
            new Color(34/255f, 177/255f, 76/255f, 1f),
            new Color(0/255f, 162/255f, 232/255f, 1f),
            new Color(63/255f, 72/255f, 204/255f, 1f),
            new Color(163/255f, 73/255f, 164/255f, 1f),

            new Color(255/255f, 255/255f, 255/255f, 1f),
            new Color(195/255f, 195/255f, 195/255f, 1f),
            new Color(255/255f, 174/255f, 201/255f, 1f),
            new Color(255/255f, 201/255f, 14/255f, 1f),
            new Color(239/255f, 228/255f, 176/255f, 1f),
            new Color(181/255f, 230/255f, 029/255f, 1f),
            new Color(153/255f, 217/255f, 234/255f, 1f),
            new Color(112/255f, 146/255f, 190/255f, 1f),
            new Color(200/255f, 191/255f, 231/255f, 1f)
        };

        private Transform parent;
        private void Start()
        {
            parent = buttonTemplate.transform.parent;
            buttonTemplate.gameObject.SetActive(false);

            foreach (Color c in colors)
            {
                MakeButton(c, OnColor);
            }
        }

        private void OnColor(Color c)
        {
            if (target == TargetValue.foreground)
            {
                Globals.foreground = c;
            }
            else if (target == TargetValue.background)
            {
                Globals.background = c;
            }

            Events.OnSavedUserDataUpdate?.Invoke();
        }

        private void MakeButton(Color c, System.Action<Color> onClick)
        {
            Button button = GameObject.Instantiate(buttonTemplate, parent);
            button.gameObject.GetComponent<Image>().color = c;
            button.gameObject.SetActive(true);
            button.onClick.AddListener(() => onClick?.Invoke(c));
        }
    }
}