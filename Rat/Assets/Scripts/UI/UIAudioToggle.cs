using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rat
{
    public class UIAudioToggle : RatMonoBehaviour
    {
        [SerializeField] Button muteButton;
        [SerializeField] Image muteButtonImage;
        [SerializeField] Sprite mutedImage;
        [SerializeField] Sprite noiseImage;

        void OnEnable()
        {
            UpdateButtonVisual();
            muteButton.onClick.AddListener(ToggleMuted);
            Events.OnChangeMuteStatus += UpdateButtonVisual;
        }

        private void OnDisable()
        {
            Events.OnChangeMuteStatus -= UpdateButtonVisual;
            muteButton.onClick.RemoveListener(ToggleMuted);
        }

        private void ToggleMuted()
        {
            Globals.IsMuted = !Globals.IsMuted;
        }

        private void UpdateButtonVisual()
        {
            muteButtonImage.sprite = Globals.IsMuted ? mutedImage : noiseImage;
        }
    }
}