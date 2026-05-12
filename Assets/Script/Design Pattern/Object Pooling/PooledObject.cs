using UnityEngine;

public class PooledObject : MonoBehaviour
{
    ObjectPooling instance { get; set; }
    public ObjectPooling Instance { get => instance; set => instance = value; }


    public void Release(string name)
    {
        if (instance != null)
        {
            Debug.Log("Release");
            instance.ReturnToPool(name, this);
        }
    }
}
