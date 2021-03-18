using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rat
{
    public class RatCutscene : RatMonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Camera cam;
        [SerializeField] private Color target;
        [SerializeField] private float camFadeDuration;
        [SerializeField] private float camShake;
        [SerializeField] private float camShakeMax;
        [SerializeField] private float camShakeDuration;
        [Header("Rat")]
        [SerializeField] private Vector3 startPos;
        [SerializeField] private Vector3 startRot;
        [SerializeField] private Vector3 startScale;
        [SerializeField] private float duration;
        [SerializeField] private AnimationCurve curve;
        [Header("Hat")]
        [SerializeField] private GameObject hat;
        [Header("FX")]
        [SerializeField] private GameObject confetti;
        [Header("Navigation")]
        [SerializeField] private string targetExitScene;

        // Cam
        private float timeSoFarCam;
        private Color startColor;
        private Vector3 cameraStartPos;
        private bool shakeEnabled;
        private float timeSoFarShake;

        // Rat
        private float timeSoFar;
        private Vector3 targetPos;
        private Vector3 targetRot;
        private Vector3 targetScale;


        private void Awake()
        {
            // Cam
            this.startColor = cam.backgroundColor;
            this.cameraStartPos = cam.transform.position;

            // Rat
            this.targetPos = this.transform.position;
            this.targetRot = this.transform.rotation.eulerAngles;
            this.targetScale = this.transform.localScale;

            hat.SetActive(false);
            confetti.gameObject.SetActive(false);
        }

        private void Start()
        {
            Events.OnPlaySound?.Invoke(AudioLabel.MusicSting, AudioType.Music);
        }

        private void Update()
        {
            // General: Cam fade
            if (timeSoFarCam < camFadeDuration)
            {
                timeSoFarCam += Time.deltaTime;

                if (timeSoFarCam > camFadeDuration)
                {
                    timeSoFarCam = camFadeDuration;
                }

                float t = timeSoFarCam / camFadeDuration;
                cam.backgroundColor = Color.Lerp(startColor, target, t);
            }

            // General: Cam shake
            if (camShake > 0.001f)
            {
                float randX = Random.Range(-camShake, camShake);
                float randY = Random.Range(-camShake, camShake);
                float randZ = Random.Range(-camShake, camShake);

                cam.transform.position = cameraStartPos + new Vector3(randX, randY, randZ);
            }
            else
            {
                cam.transform.position = cameraStartPos;
            }

            // Step 1: pos lerp
            if (timeSoFar < duration)
            {
                timeSoFar += Time.deltaTime;

                bool lastStep = false;
                if (timeSoFar >= duration)
                {
                    lastStep = true;
                    timeSoFar = duration;
                }

                float rawT = timeSoFar / duration;
                float t = curve.Evaluate(rawT);

                this.transform.position = Vector3.Lerp(startPos, targetPos, t);
                this.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRot, targetRot, t));
                this.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

                if (lastStep)
                {
                    shakeEnabled = true;
                }
            }

            // Step 2: lerp cam shake
            if (shakeEnabled && timeSoFarShake < camShakeDuration)
            {
                timeSoFarShake += Time.deltaTime;

                bool lastStep = false;
                if (timeSoFarShake >= camShakeDuration)
                {
                    lastStep = true;
                    timeSoFarShake = camShakeDuration;
                }

                float t = timeSoFarShake / camShakeDuration;
                camShake = Mathf.Lerp(0, camShakeMax, t);

                if (lastStep)
                {
                    StartCoroutine(SceneFinish());
                }
            }
        }

        private IEnumerator SceneFinish()
        {
            camShake = 0;
            Events.OnPlaySound?.Invoke(AudioLabel.None, AudioType.Music);
            yield return new WaitForSeconds(0.05f);
            Events.OnPlaySound?.Invoke(AudioLabel.SoundBubblePop, AudioType.SFX);
            GameValues.Instance.hatEnabled = true;
            hat.SetActive(true);
            yield return new WaitForSeconds(1);
            Events.OnPlaySound?.Invoke(AudioLabel.SoundCheering, AudioType.SFX);
            confetti.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(targetExitScene);
        }
    }
}