using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rat
{
    [System.Serializable]
    public enum AudioType
    {
        Music,
        SFX
    }

    [System.Serializable]
    public enum AudioLabel
    {
        None = -1,

        MusicEnergy = 0,
        SoundRatClick,
        SoundBubblePop,
        SoundCheering,
        MusicSting,
        MusicChill
    }

    [System.Serializable]
    public struct AudioPair
    {
        public AudioLabel label;
        public AudioClip source;
    }

    // Audio playback singleton
    public class GameAudio : RatMonoBehaviour
    {
        // Constants
        public const float volumeMusic = 0.7f;
        public const float volumeSFX = 1f;

        // Public variables
        public List<AudioPair> Sounds;

        // Private variables
        private AudioSource sourceMusic;
        private AudioSource sourceSFX;
        private AudioLabel currentLabel;

        // Singleton stuff
        public static GameAudio Instance { get; private set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
                Init();
            }
            else
            {
                Destroy(this);
            }
        }

        // Initializes the audio system
        void Init()
        {
            sourceMusic = gameObject.AddComponent<AudioSource>();
            sourceMusic.loop = true;

            sourceSFX = gameObject.AddComponent<AudioSource>();

            Events.OnPlaySound += (label, type) =>
            {
                switch(type)
                {
                    case AudioType.Music:
                        if (label == AudioLabel.None)
                        {
                            sourceMusic.Stop();
                            currentLabel = AudioLabel.None;
                        }
                        else
                        {
                            if (label != currentLabel)
                            {
                                PlayOn(label, sourceMusic, volumeMusic);
                                currentLabel = label;
                            }
                        }
                        break;

                    case AudioType.SFX:
                        if (label == AudioLabel.None)
                        {
                            sourceSFX.Stop();
                        }
                        else
                        {
                            PlayOn(label, sourceSFX, volumeSFX);
                        }
                        break;
                }
            };
        }

        // Randomly plays something from the specified label on the specified managed source.
        void PlayOn(AudioLabel label, AudioSource source, float volume)
        {
            List<AudioPair> clips = new List<AudioPair>();

            foreach (var clip in Sounds)
            {
                if (clip.label == label)
                    clips.Add(clip);
            }

            AudioClip target = clips[Random.Range(0, clips.Count)].source;

            source.Stop();
            source.volume = volume;
            source.clip = target;
            source.Play();
        }
    }
}