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
    
    public event Action ActiveFadeOnEvent;

    public event Action ActiveFadeOutEvent;

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
    
    public void ActiveFadeOnSystemUI()
    {
        ActiveFadeOnEvent?.Invoke();
    }
    
    public void ActiveFadeOutSystemUI()
    {
        ActiveFadeOutEvent?.Invoke();
    }
    
    public void HandleActiveSystemUI()
    {
        systemUI.SetActive(!systemUI.activeInHierarchy);
    }

    IEnumerator WaitToActiveCheckPoint()
    {
        ActiveSystemUIEvent?.Invoke();
        yield return new WaitForSeconds(3f);
        systemUI.SetActive(!systemUI.activeInHierarchy);
    }
}
