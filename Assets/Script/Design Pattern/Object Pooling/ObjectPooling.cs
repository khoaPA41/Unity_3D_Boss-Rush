using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Design_Pattern.Object_Pooling
{
    [Serializable]
    public class PoolItem
    {
        public PooledObject pooledObject;
        public string objectName;
        [Range(1, 50)] public uint size;
    }


    public class ObjectPooling : MonoBehaviour
    {
        [SerializeField] private List<PoolItem> poolItemList;

        private Dictionary<string, Stack<PooledObject>> pooledObjectDict;

        private List<GameObject> parentObjectList;

        private void Start()
        {
            parentObjectList = new List<GameObject>();
            foreach (var item in poolItemList)
            {
                var parentType = new GameObject(item.objectName + "_Pool");
                parentType.transform.SetParent(this.transform);
                parentObjectList.Add(parentType.gameObject);
            }

            Setup();
        }

        private void Setup()
        {
            if (poolItemList.Count == 0)
            {
                return;
            }

            pooledObjectDict = new Dictionary<string, Stack<PooledObject>>();
            foreach (var item in poolItemList)
            {
                var pooledObjectStack = new Stack<PooledObject>();
                var parent =
                    parentObjectList.Find(temp => temp.name.Substring(0, temp.name.Length - 5) == item.objectName);
                for (var i = 0; i < item.size; i++)
                {
                    var newItem = Instantiate(item.pooledObject, item.pooledObject.transform.position,
                        Quaternion.identity);
                    newItem.Instance = this;
                    newItem.name = item.objectName;
                    newItem.transform.SetParent(parent.transform);
                    newItem.gameObject.SetActive(false);
                    pooledObjectStack.Push(newItem);
                }

                pooledObjectDict.Add(item.objectName, pooledObjectStack);
            }
        }


        public PooledObject GetPooledObject(string objectName, Vector3 objectPosition)
        {
            if (string.IsNullOrEmpty(objectName) || !pooledObjectDict.ContainsKey(objectName))
            {
                return null;
            }

            if (pooledObjectDict[objectName].Count == 0)
            {
                var newObject =
                    Instantiate(poolItemList.Find(itemPool => itemPool.objectName == objectName).pooledObject);
                newObject.Instance = this;
                newObject.name = objectName;
                newObject.transform.SetParent(parentObjectList
                    .Find(parent => parent.name.Substring(0, parent.name.Length - 5) == objectName).transform);
                newObject.transform.position = objectPosition;
                newObject.gameObject.SetActive(true);
                return newObject;
            }

            var pooledObject = pooledObjectDict[objectName].Pop();
            pooledObject.transform.position = objectPosition;
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        public void ReturnToPool(string objectName, PooledObject pooledObject)
        {
            if (string.IsNullOrEmpty(objectName) || !pooledObjectDict.ContainsKey(objectName))
            {
                Destroy(pooledObject);
                return;
            }

            // if (pooledObjectDict[objectName].Contains(pooledObject)) return;
            pooledObject.gameObject.SetActive(false);
            pooledObjectDict[objectName].Push(pooledObject);
        }
    }
}