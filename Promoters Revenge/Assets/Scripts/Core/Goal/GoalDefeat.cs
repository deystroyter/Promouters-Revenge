using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Assets.Scripts;
using Assets.Scripts.Core.Damage;
using Assets.Scripts.Core.Quest;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Core.Quest
{
    public class GoalDefeat : MonoBehaviour, IGoal
    {
        public event Action OnComplete;
        public event Action<int> OnProgressChanged;
        public Sprite GoalIcon;

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
                    GameManager.Instance.OpenLevelExit();
                }
            }
        }

        private int _currentAmount = 0;

        public int RequiredAmount
        {
            get => _requiredAmount;
        }

        [SerializeField] private int _requiredAmount;
        [SerializeField] public DamageableObject.ObjectType DmgObjTargetType;

        protected void Start()
        {
            MakeDescription();
            LevelInfo.Instance.OnDamageableObjectDie += DamageableObjectDied_GoalDefeatHandler;
            UI_QuestInfoController.Instance.InitGoal(this);
        }


        private void MakeDescription()
        {
            GoalDescription += $"Defeat {_requiredAmount} ";
            switch (DmgObjTargetType)
            {
                case DamageableObject.ObjectType.Enemy:
                    GoalDescription += $"enemies.";
                    break;
                case DamageableObject.ObjectType.TestCube:
                    GoalDescription += $"test cubes.";
                    break;
                default:
                    throw new InvalidEnumArgumentException($"MakeDescription(): DmgObjTargetType - {DmgObjTargetType.ToString()}");
            }
        }

        private void DamageableObjectDied_GoalDefeatHandler(DamageableObject.ObjectType type)
        {
            CurrentAmount = LevelInfo.Instance.DamageableTypesCounter[type];
            OnProgressChanged?.Invoke(CurrentAmount);
        }
    }
}