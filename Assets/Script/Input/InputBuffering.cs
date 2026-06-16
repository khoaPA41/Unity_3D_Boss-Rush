using System;
using System.Collections.Generic;
using UnityEngine;


public enum ActionType
{
    None,
    Jump,
    Dodge,
    Attack,
}
[Serializable]
public struct BufferedAction
{
    public ActionType ActionType;
    public float BufferWindow;
}

public class InputBuffering : MonoBehaviour
{
    [Header("Input Buffering Time")] 
    [Tooltip("Setting the buffer window time here!")]
    [field:SerializeField] private List<BufferedAction> bufferedActions;
    
    
    private Dictionary<ActionType, float> _bufferWindowDictionary = new Dictionary<ActionType, float>();
    private Dictionary<ActionType, float> _lastInputTimeDictionary = new Dictionary<ActionType, float>();

    private void Awake()
    {
        /*Convert data necessary from "bufferedActions" to two dictionary*/
        foreach (var action in bufferedActions)
        {
            _bufferWindowDictionary[action.ActionType] = action.BufferWindow;
            _lastInputTimeDictionary[action.ActionType] = -1000f;
        }
    }
    
    /*register player input and assign Time.time to _lastInputTimeDictionary*/
    public void Register(ActionType actionType)
    {
        if (_lastInputTimeDictionary.ContainsKey(actionType))
        {
            _lastInputTimeDictionary[actionType] = Time.time;
        }

        // Debug.Log($"[InputBuffer] {actionType} at {Time.time}");
    }
    
    /*Try to consume the last valid input*/
    public bool TryConsume(ActionType actionType)
    {
        if(!_lastInputTimeDictionary.ContainsKey(actionType) || !_bufferWindowDictionary.ContainsKey(actionType)) return false;
        
        var lastTime = _lastInputTimeDictionary[actionType];
        var bufferWindow = _bufferWindowDictionary[actionType];
        // Debug.Log($"[lastTime] {lastTime}");
        // Debug.Log($"[Time.time] {Time.time}");
        // Debug.Log($"[final] {Time.time -  lastTime}");
        if (Time.time - lastTime <= bufferWindow)
        {
            _lastInputTimeDictionary[actionType] = -1000f;
            // Debug.Log("true");
            return true;
        }
        return false;
    }
    
}