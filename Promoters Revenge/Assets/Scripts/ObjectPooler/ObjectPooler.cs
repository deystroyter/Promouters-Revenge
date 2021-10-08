using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectPooler : MonoBehaviour
    {
        public static ObjectPooler ObjPooler;

        [Serializable]
        public struct ObjectInfo
        {
            public enum ObjectType
            {
                BulletPaper,
                Enemy
            }

            public ObjectType Type;
            public GameObject Prefab;
            public int StartCount;
        }

        [SerializeField] private List<ObjectInfo> objectsInfo;

        private Dictionary<ObjectInfo.ObjectType, Pool> pools;

        protected void Awake()
        {
            if (ObjPooler == null) ObjPooler = this;
            InitPool();
        }

        private void InitPool()
        {
            pools = new Dictionary<ObjectInfo.ObjectType, Pool>();

            var emptyGameObj = new GameObject();
            foreach (var obj in objectsInfo)
            {
                var container = Instantiate(emptyGameObj, transform, false);
                container.name = obj.Type.ToString();

                pools[obj.Type] = new Pool(container.transform);

                for (int i = 0; i < obj.StartCount; i++)
                {
                    var go = InstantiateObject(obj.Type, container.transform);
                    pools[obj.Type].Objects.Enqueue(go);
                }
            }

            Destroy(emptyGameObj);
        }

        private GameObject InstantiateObject(ObjectInfo.ObjectType type, Transform parent)
        {
            var go = Instantiate(objectsInfo.Find(x => x.Type == type).Prefab, parent);
            go.SetActive(false);
            return go;
        }

        public GameObject GetObject(ObjectInfo.ObjectType type)
        {
            //Debug.Log(pools[type].Objects.Count);
            var obj = pools[type].Objects.Count > 0
                ? pools[type].Objects.Dequeue()
                : InstantiateObject(type, pools[type].Container);

            obj.SetActive(true);
            return obj;
        }

        public int GetCountOfQueueObjects(ObjectInfo.ObjectType type)
        {
            return pools[type].Objects.Count;
        }

        public void DestroyObject(GameObject obj)
        {
            obj.SetActive(false);
            pools[obj.GetComponent<IPooledObject>().Type].Objects.Enqueue(obj);
        }
    }
}