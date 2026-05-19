using UnityEngine;

namespace Script.Design_Pattern.Object_Pooling
{
    public class PooledObject : MonoBehaviour
    {
        ObjectPooling instance { get; set; }

        public ObjectPooling Instance
        {
            get => instance;
            set => instance = value;
        }


        public void Release(string name)
        {
            instance?.ReturnToPool(name, this);
        }
    }
}