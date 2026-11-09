using Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WorldUIManager : MonoBehaviour
    {
        public static WorldUIManager Instance;

        [Header("System UI")][SerializeField] private GameObject systemUI;
        [Header("Settings UI")][SerializeField] private GameObject settingsUI;

        [Header("Tab UI")][SerializeField] private List<GameObject> tabUIList;

        [Header("Defeat Title UI")]
        [SerializeField] private Image defeatBackground;
        [SerializeField] private TextMeshProUGUI defeatText;

        public event Action ActiveSystemUIEvent;
        public bool isActiveSettingsUI;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            tabUIList[0].SetActive(true);
        }

        /******************* Exit *******************/
        public void ExitGame()
        {
            GameManager.Instance.ExitGame();
        }

        public void ExitToTitle()
        {
            GameManager.Instance.ExitToTitle();
        }

        /******************* Checkpoint *******************/
        public void ShowTabUI(string tabName)
        {
            foreach (var tab in tabUIList)
            {
                tab.SetActive(tabName == tab.name);
            }
        }

        /******************* UI Sound *******************/
        public void PlayClickSound1()
        {
            AudioManager.Instance.PlayUISound(AudioManager.Instance.buttonSoundClick_1);
        }

        public void PlayClickSound2()
        {
            AudioManager.Instance.PlayUISound(AudioManager.Instance.buttonSoundClick_2);
        }

        public void PlayClickSound3()
        {
            AudioManager.Instance.PlayUISound(AudioManager.Instance.buttonSoundClick_3);
        }

        public void PlayClickSound4()
        {
            AudioManager.Instance.PlayUISound(AudioManager.Instance.buttonSoundClick_4);
        }

        public void PlayHoldSound1()
        {
            AudioManager.Instance.PlayUISound(AudioManager.Instance.buttonSoundHold_1);
        }

        public void PlayHoldSound2()
        {
            AudioManager.Instance.PlayUISound(AudioManager.Instance.buttonSoundHold_2);
        }

        public void PlayHoldSound3()
        {
            AudioManager.Instance.PlayUISound(AudioManager.Instance.buttonSoundHold_3);
        }

        public void PlayHoldSound4()
        {
            AudioManager.Instance.PlayUISound(AudioManager.Instance.buttonSoundHold_4);
        }

        /****************************************************************************************/
        public void ShowSystemUI()
        {
            ActiveSystemUIEvent?.Invoke();
        }

        public void HandleActiveSystemUI()
        {
            systemUI.SetActive(!systemUI.activeInHierarchy);
        }

        public void ShowSettingsUI()
        {
            isActiveSettingsUI = true;
            settingsUI.SetActive(!settingsUI.activeInHierarchy);
            if (!settingsUI.activeInHierarchy)
            {
                isActiveSettingsUI = false;
            }
        }

        /********************************************Fade Defeat*********************************************/

        public void StartCoroutineTitleFade(float currentOpacity, float targetOpacity, float fadeDuration)
        {
            StartCoroutine(TitleFade(currentOpacity, targetOpacity, fadeDuration));

        }

        public void StartCoroutineBackgroundFade(float currentOpacity, float targetOpacity, float fadeDuration)
        {
            StartCoroutine(BackgroundFade(currentOpacity, targetOpacity, fadeDuration));

        }

        private IEnumerator TitleFade(float currentOpacity, float targetOpacity, float fadeDuration)
        {
            defeatText.gameObject.SetActive(true);
            var timeElapsed = 0f;
            var current = defeatText.color;
            while (timeElapsed < fadeDuration)
            {
                var timePercentage = timeElapsed / fadeDuration;
                var newColor = Mathf.Lerp(currentOpacity, targetOpacity, timePercentage);
                current.a = newColor;
                defeatText.color = current;
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            current.a = targetOpacity;
            defeatText.color = current;
        }

        private IEnumerator BackgroundFade(float currentOpacity, float targetOpacity, float fadeDuration)
        {
            var timeElapsed = 0f;
            var backgroundColor = defeatBackground.color;

            while (timeElapsed < fadeDuration)
            {
                var timePercentage = timeElapsed / fadeDuration;
                var newColor = Mathf.Lerp(currentOpacity, targetOpacity, timePercentage);
                backgroundColor.a = newColor;
                defeatBackground.color = backgroundColor;
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            backgroundColor.a = targetOpacity;
            defeatBackground.color = backgroundColor;
        }
    }
}