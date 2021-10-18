using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Core.Damage;
using Assets.Scripts.Core.Quest;
using UnityEngine;

namespace Assets.Scripts.Core.Quest
{
    public class GoalDefeat : MonoBehaviour, IGoal
    {
        private LevelManager _levelManager;

        public event IGoal.GoalCompleted OnComplete;

        public string GoalDescription { get; private set; }
        public int CurrentAmount
        {
            get => _currentAmount;
            set
            {
                _currentAmount = Mathf.Clamp(value, 0, _requiredAmount);
                if (_currentAmount >= _requiredAmount)
                {
                    OnComplete?.Invoke();
                }
            }
        }
        private int _currentAmount = 0;
        [SerializeField] private int _requiredAmount;
        [SerializeField] public DamageableObject.ObjectType DmgObjTargetType;

        // Start is called before the first frame update
        void Start()
        {
            _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            _levelManager.OnDamageableObjectDie += DamageableObjectDied_GoalDefeatHandler;
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void MakeDescription()
        {
            GoalDescription += $"Устраните {_requiredAmount} ";
            switch (DmgObjTargetType)
            {
                case DamageableObject.ObjectType.Enemy:
                    GoalDescription += $"противников.";
                    break;
                case DamageableObject.ObjectType.TestCube:
                    GoalDescription += $"тестовых кубиков.";
                    break;
                default:
                    GoalDescription += $"UNKNOWN DMG OBJECT :((";
                    break;
            }
        }

        private void DamageableObjectDied_GoalDefeatHandler(DamageableObject.ObjectType type)
        {
            CurrentAmount = _levelManager.DamageableTypesCounter[type];
        }
    }
}