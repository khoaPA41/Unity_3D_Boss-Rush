using System;
using UnityEngine;

public class TickSystem : MonoBehaviour
{
    public static event Action<float> OnUpdateSystem;

    void Update()
    {
        OnUpdateSystem?.Invoke(Time.deltaTime);
    }
}
