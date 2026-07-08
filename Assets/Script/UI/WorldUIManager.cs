using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIManager : MonoBehaviour
{
    public static WorldUIManager instance;

    [Header("System UI")]
    [SerializeField] private GameObject systemUI;

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
    
    /******************* Sound *******************/


    /******************* Graphics *******************/

    /******************* Checkpoint *******************/
    public void ActiveTabUI(string tabName)
    {
        foreach (var tab in tabUIList)
        {
            tab.SetActive(tabName == tab.name);
        }
    }

    public void ActiveSystemUI()
    {
        ActiveSystemUIEvent?.Invoke();
    }
    
    public void HandleActiveSystemUI()
    {
        systemUI.SetActive(!systemUI.activeInHierarchy);
    }
}
