using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class PoolItem
{
    public PooledObject pooledObject;
    public string objectName;
    [Range(1, 50)] public uint size;
}



public class ObjectPooling : MonoBehaviour
{
    [SerializeField] List<PoolItem> poolItemList;

    Dictionary<string, Stack<PooledObject>> pooledObjectDict;

    void Start()
    {
        Setup();
    }

    void Setup()
    {
        if (poolItemList.Count == 0) { return; }
        pooledObjectDict = new Dictionary<string, Stack<PooledObject>>();
        foreach (var item in poolItemList)
        {
            Stack<PooledObject> pooledObjectStack = new Stack<PooledObject>();

            GameObject parentType = new GameObject(item.objectName + "_Pool");
            parentType.transform.SetParent(this.transform);

            for (int i = 0; i < item.size; i++)
            {
                PooledObject newItem = Instantiate(item.pooledObject);
                newItem.name = item.objectName;
                newItem.transform.SetParent(parentType.transform);
                newItem.gameObject.SetActive(false);
                pooledObjectStack.Push(newItem);
            }
            pooledObjectDict.Add(item.objectName, pooledObjectStack);
        }
    }


    public PooledObject GetPooledObject(string objectName, Vector3 objectPosition)
    {
        if (string.IsNullOrEmpty(objectName) || !pooledObjectDict.ContainsKey(objectName)) { return null; }


        if (pooledObjectDict[objectName].Count == 0)
        {
            PooledObject newObject = Instantiate(poolItemList.Find(itemPool => itemPool.objectName == objectName).pooledObject);
            newObject.transform.position = objectPosition;
            newObject.gameObject.SetActive(true);
            return newObject;

        }

        PooledObject pooledObject = pooledObjectDict[objectName].Pop();
        pooledObject.transform.position = objectPosition;
        pooledObject.gameObject.SetActive(true);
        return pooledObject;

    }

    public void ReturnToPool()
    {

    }

}
