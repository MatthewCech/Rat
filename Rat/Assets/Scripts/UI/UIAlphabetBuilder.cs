using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rat
{
    public class UIAlphabetBuilder : RatMonoBehaviour
    {
        enum TargetValue
        {
            first,
            second
        }

        [SerializeField] private TargetValue target;
        [SerializeField] private Button buttonTemplate;

        private Transform parent;
        private void Start()
        {
            parent = buttonTemplate.transform.parent;
            buttonTemplate.gameObject.SetActive(false);

            char symbol = 'a';
            while (symbol <= 'z')
            {
                MakeButton(symbol++, SetLetter);
            }

            MakeButton('_', SetLetter);
        }

        private void SetLetter(char letter)
        {
            if(target == TargetValue.first)
            {
                if(Globals.savedTag == null)
                {
                    Globals.savedTag = $"{letter}_";
                }
                else
                {
                    Globals.savedTag = $"{letter}{Globals.savedTag[1]}";
                }
            }
            else if (target == TargetValue.second)
            {
                if (Globals.savedTag == null)
                {
                    Globals.savedTag = $"_{letter}";
                }
                else
                {
                    Globals.savedTag = $"{Globals.savedTag[0]}{letter}";
                }
            }

            Events.OnSavedUserDataUpdate?.Invoke();
        }

        private void MakeButton(char letter, System.Action<char> onClick)
        {
            Button button = GameObject.Instantiate(buttonTemplate, parent);
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = letter.ToString();
            button.gameObject.SetActive(true);
            button.onClick.AddListener(() => onClick?.Invoke(letter));
        }
    }
}
