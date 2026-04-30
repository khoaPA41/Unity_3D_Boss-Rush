using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> CancelTargetEvent;

    private void OnDisable()
    {
        OnCancelTarget();
    }
    void OnCancelTarget()
    {
        CancelTargetEvent?.Invoke(this);
    }
}
