using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Rat
{
    public class UILeaderboardEntry : RatMonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI entryLabel;
        [SerializeField] private TextMeshProUGUI entryNumber;
        [SerializeField] private TextMeshProUGUI entryValue;
        [SerializeField] private TextMeshProUGUI tag;
        [SerializeField] private Image bg;

        public void SetValue(string label, string value, int number, Color foreground, Color background)
        {
            if(entryLabel)
            {
                entryLabel.text = "rat";
            }
            entryValue.text = value;

            // Set tag values
            entryNumber.text = number.ToString();
            tag.text = label;
            tag.color = foreground;
            bg.color = background;
        }
    }
}