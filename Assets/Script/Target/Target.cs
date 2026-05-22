using System;
using UnityEngine;

namespace Script.Target
{
    public class Target : MonoBehaviour
    {
        public event Action<Target> CancelTargetEvent;

        private void OnDisable()
        {
            OnCancelTarget();
        }
        private void OnCancelTarget()
        {
            CancelTargetEvent?.Invoke(this);
        }
    }
}
