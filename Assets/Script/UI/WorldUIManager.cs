using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class WorldUIManager : MonoBehaviour
{
    public static WorldUIManager instance;

    [Header("System UI")] [SerializeField] private GameObject systemUI;

    [Header("Tab UI")] [SerializeField] private List<GameObject> tabUIList;


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
}