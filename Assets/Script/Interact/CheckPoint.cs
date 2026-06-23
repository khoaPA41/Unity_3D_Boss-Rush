using System;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private InputReader _inputReader;

    private void Start()
    {
        _inputReader = GetComponent<InputReader>();
    }

    private void ActiveCheckPointUI()
    {
        WorldUIManager.instance.ActiveSystemUI();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            _inputReader.ActiveCheckPointAction += ActiveCheckPointUI;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            _inputReader.ActiveCheckPointAction -= ActiveCheckPointUI;
        }
    }
}