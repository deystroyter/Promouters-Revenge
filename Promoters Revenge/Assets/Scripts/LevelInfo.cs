using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Damage;
using Assets.Scripts.Core.Collect;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelInfo : MonoBehaviour
    {
        public static LevelInfo Instance;

        public GameObject Player;
        public GameObject Weapon;
        public GameObject LevelExit;

        public Dictionary<DamageableObject.ObjectType, int> DamageableTypesCounter = new Dictionary<DamageableObject.ObjectType, int>();
        public delegate void DamageableObjectDieEvent(DamageableObject.ObjectType dmgObjType);
        public event DamageableObjectDieEvent OnDamageableObjectDie;

        public Dictionary<CollectibleObject.ObjectType, int> CollectibleTypesCounter = new Dictionary<CollectibleObject.ObjectType, int>();
        public delegate void CollectibleObjectDieEvent(CollectibleObject.ObjectType collectObjType);
        public event CollectibleObjectDieEvent OnCollectibleObjectDie;


        protected void Awake()
        {
            Instance = this;
            LevelExit.SetActive(false);
        }

        protected void Update()
        {
        }


        public void DamageableObjectDied(DamageableObject.ObjectType type)
        {
            if (!Instance.DamageableTypesCounter.ContainsKey(type))
            {
                Instance.DamageableTypesCounter.Add(type, 0);
            }

            Instance.DamageableTypesCounter[type]++;
            Debug.Log(type + " || " + Instance.DamageableTypesCounter[type]);
            Instance.OnDamageableObjectDie?.Invoke(type);
        }

        public void CollectibleObjectDied(CollectibleObject.ObjectType type)
        {
            if (!Instance.CollectibleTypesCounter.ContainsKey(type))
            {
                Instance.CollectibleTypesCounter.Add(type, 0);
            }

            Instance.CollectibleTypesCounter[type]++;
            Debug.Log(type + " || " + Instance.CollectibleTypesCounter[type]);
            Instance.OnCollectibleObjectDie?.Invoke(type);
        }

        public void OpenLevelExit()
        {
            LevelExit.SetActive(true);
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}