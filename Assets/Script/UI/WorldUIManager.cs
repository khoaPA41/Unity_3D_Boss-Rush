using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class WorldUIManager : MonoBehaviour
{
    public static WorldUIManager instance;

    [Header("System UI")] [SerializeField] private GameObject systemUI;

    [Header("Tab UI")] [SerializeField] private List<GameObject> tabUIList;

    [Header("Defeat Title UI")] 
    [SerializeField] private Image defeatBackground;
    [SerializeField] private TextMeshProUGUI defeatText;
    public event Action ActiveSystemUIEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        tabUIList[0].SetActive(true);
    }

    /******************* Exit *******************/
    public void ExitGame()
    {
        GameManagers.Instance.ExitGame();
    }

    public void ExitToTitle()
    {
        GameManagers.Instance.ExitToTitle();
    }
    
    /******************* Checkpoint *******************/
    public void ActiveTabUI(string tabName)
    {
        foreach (var tab in tabUIList)
        {
            tab.SetActive(tabName == tab.name);
        }
    }

    /******************* UI Sound *******************/

    public void PlayClickSound1()
    {
        AudioManagers.Instance.PlayUISound(AudioManagers.Instance.buttonSoundClick_1);
    }

    public void PlayClickSound2()
    {
        AudioManagers.Instance.PlayUISound(AudioManagers.Instance.buttonSoundClick_2);
    }

    public void PlayClickSound3()
    {
        AudioManagers.Instance.PlayUISound(AudioManagers.Instance.buttonSoundClick_3);
    }

    public void PlayClickSound4()
    {
        AudioManagers.Instance.PlayUISound(AudioManagers.Instance.buttonSoundClick_4);
    }

    public void PlayHoldSound1()
    {
        AudioManagers.Instance.PlayUISound(AudioManagers.Instance.buttonSoundHold_1);
    }

    public void PlayHoldSound2()
    {
        AudioManagers.Instance.PlayUISound(AudioManagers.Instance.buttonSoundHold_2);
    }

    public void PlayHoldSound3()
    {
        AudioManagers.Instance.PlayUISound(AudioManagers.Instance.buttonSoundHold_3);
    }

    public void PlayHoldSound4()
    {
        AudioManagers.Instance.PlayUISound(AudioManagers.Instance.buttonSoundHold_4);
    }

/****************************************************************************************/
    public void ActiveSystemUI()
    {
        ActiveSystemUIEvent?.Invoke();
    }

    public void HandleActiveSystemUI()
    {
        systemUI.SetActive(!systemUI.activeInHierarchy);
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
            var TimePercentage = timeElapsed / fadeDuration;
            var newColor = Mathf.Lerp(currentOpacity, targetOpacity, TimePercentage);
            backgroundColor.a =  newColor;
            defeatBackground.color = backgroundColor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        backgroundColor.a =  targetOpacity;
        defeatBackground.color = backgroundColor;
    }
}