using Script.Design_Pattern.Object_Pooling;
using UnityEngine;


public class Test : MonoBehaviour
{
    public ObjectPooling pooledObject;

    public InputReader inputReader;

    void Start()
    {
    }

    void Update()
    {
        if (inputReader.IsSprint)
        {
            pooledObject.GetPooledObject("Enemy_1", transform.position);

        }

    }
}
