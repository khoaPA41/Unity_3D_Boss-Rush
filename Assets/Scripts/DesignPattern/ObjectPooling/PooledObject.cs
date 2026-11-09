using UnityEngine;

namespace DesignPattern.ObjectPooling
{
    public class PooledObject : MonoBehaviour
    {
        ObjectPoolManager instance { get; set; }

        private bool isReleased;
        private void OnEnable() => isReleased = false;
        public ObjectPoolManager Instance
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