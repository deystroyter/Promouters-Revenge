using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Damage;
using Assets.Scripts.Core.Collect;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager _instance;

        public GameObject Player;
        public GameObject Weapon;
        public GameObject LevelExit;

        public Dictionary<DamageableObject.ObjectType, int> DamageableTypesCounter = new Dictionary<DamageableObject.ObjectType, int>();
        public delegate void DamageableObjectDieEvent(DamageableObject.ObjectType dmgObjType);
        public event DamageableObjectDieEvent OnDamageableObjectDie;

        public Dictionary<CollectibleObject.ObjectType, int> CollectibleTypesCounter = new Dictionary<CollectibleObject.ObjectType, int>();
        public delegate void CollectibleObjectDieEvent(CollectibleObject.ObjectType collectObjType);
        public event CollectibleObjectDieEvent OnCollectibleObjectDie;


        public bool IsPaused = false;
        public GameObject PauseMenu;

        protected void Awake()
        {
            MakeSingleton();
            LevelExit.SetActive(false);
        }

        protected void Update()
        {
            GamePauseLogic();
        }

        private void GamePauseLogic()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        public void ResumeGame()
        {
            PauseMenu.SetActive(false);
            IsPaused = false;
            Time.timeScale = 1f;
        }

        public void PauseGame()
        {
            PauseMenu.SetActive(true);
            IsPaused = true;
            Time.timeScale = 0f;
        }

        public void DamageableObjectDied(DamageableObject.ObjectType type)
        {
            if (!_instance.DamageableTypesCounter.ContainsKey(type))
            {
                _instance.DamageableTypesCounter.Add(type, 0);
            }

            _instance.DamageableTypesCounter[type]++;
            Debug.Log(type + " || " + _instance.DamageableTypesCounter[type]);
            _instance.OnDamageableObjectDie?.Invoke(type);
        }

        public void CollectibleObjectDied(CollectibleObject.ObjectType type)
        {
            if (!_instance.CollectibleTypesCounter.ContainsKey(type))
            {
                _instance.CollectibleTypesCounter.Add(type, 0);
            }

            _instance.CollectibleTypesCounter[type]++;
            Debug.Log(type + " || " + _instance.CollectibleTypesCounter[type]);
            _instance.OnCollectibleObjectDie?.Invoke(type);
        }

        public void OpenLevelExit()
        {
            LevelExit.SetActive(true);
        }

        private void MakeSingleton()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}