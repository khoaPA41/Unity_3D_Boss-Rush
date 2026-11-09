using UnityEngine;

namespace DesignPattern.ObjectPooling
{
    public class PooledObject : MonoBehaviour
    {
        ObjectPooling instance { get; set; }

        private bool isReleased;
        private void OnEnable() => isReleased = false;
        public ObjectPooling Instance
        {
            get => instance;
            set => instance = value;
        }


        public void Release(string name)
        {
            if (isReleased) return;
            instance?.ReturnToPool(name, this);
            isReleased = true;
        }
    }
}