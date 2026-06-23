using System.Collections.Generic;
using UnityEngine;

public class WorldUIManager : MonoBehaviour
{
    public static WorldUIManager instance;

    [Header("System UI")]
    [SerializeField] private GameObject systemUI;

    [Header("Tab UI")] [SerializeField] private List<GameObject> tabUIList;

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
        systemUI.SetActive(!systemUI.activeInHierarchy);
    }
}
