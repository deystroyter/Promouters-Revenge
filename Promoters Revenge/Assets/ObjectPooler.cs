using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler current;
    public bool canGrow;

    [Serializable]
    public struct ObjectInfo
    {
        public enum ObjectType
        {
            BULLET_PAPER,
            ENEMY
        }

        public ObjectType Type;
        public GameObject Prefab;
        public int StartCount;
    }

    [SerializeField]
    private List<ObjectInfo> objectsInfo;

    private Dictionary<ObjectInfo.ObjectType, Pool> pools;

    private void Awake()
    {
        if (current == null) current = this;
        InitPool();
    }

    private void InitPool()
    {
        pools = new Dictionary<ObjectInfo.ObjectType, Pool>();

        var emptyGO = new GameObject();
        foreach (var obj in objectsInfo)
        {
            var container = Instantiate(emptyGO, transform, false);
            container.name = obj.Type.ToString();

            pools[obj.Type] = new Pool(container.transform);

            for (int i = 0; i < obj.StartCount; i++)
            {
                var go = InstantiateObject(obj.Type, container.transform);
                pools[obj.Type].Objects.Enqueue(go);
            }
        }
        Destroy(emptyGO);
    }

    private GameObject InstantiateObject(ObjectInfo.ObjectType type, Transform parent)
    {
        var go = Instantiate(objectsInfo.Find(x => x.Type == type).Prefab, parent);
        go.SetActive(false);
        return go;
    }

    public GameObject GetObject(ObjectInfo.ObjectType type)
    {
        Debug.Log(pools[type].Objects.Count);
        var obj = pools[type].Objects.Count > 0 ?
            pools[type].Objects.Dequeue() : null;//InstantiateObject(type, pools[type].Container);

        obj.SetActive(true);
        return obj;
    }

    public void DestroyObject(GameObject obj)
    {
        obj.SetActive(false);
        pools[obj.GetComponent<IPooledObject>().Type].Objects.Enqueue(obj);
    }

    //private void Start()
    //{
    //    pooledObjects = new List<GameObject>();
    //    for (int i = 0; i < pooledAmount; i++)
    //    {
    //        GameObject obj = (GameObject)Instantiate(pooledObject);
    //        obj.SetActive(false);
    //        pooledObjects.Add(obj);
    //    }
    //}

    //public GameObject GetPooledObject()
    //{
    //    for (int i = 0; i < pooledObjects.Count; i++)
    //    {
    //        if (!pooledObjects[i].activeInHierarchy)
    //        {
    //            return pooledObjects[i];
    //        }
    //    }
    //    if (canGrow)
    //    {
    //        GameObject obj = (GameObject)Instantiate(pooledObject);
    //        pooledObjects.Add(obj);
    //        return obj;
    //    }
    //    return null;
    //}
}