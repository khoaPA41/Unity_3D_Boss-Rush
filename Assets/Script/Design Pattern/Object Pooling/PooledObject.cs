using UnityEngine;

public class PooledObject : MonoBehaviour
{
    ObjectPooling instance { get; set; }
    public ObjectPooling Instance { get => instance; set => instance = value; }


    public void Release()
    {
        if (Instance != null)
        {
            Instance.ReturnToPool();
        }
    }
}
