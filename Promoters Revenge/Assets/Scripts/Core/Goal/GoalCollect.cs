using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Core.Collect;
using Assets.Scripts.UI;

namespace Assets.Scripts.Core.Quest
{
    public class GoalCollect : MonoBehaviour, IGoal
    {
        public event Action OnComplete;
        public event Action<int> OnProgressChanged;
        public Sprite GoalIcon;

        public string GoalDescription { get; private set; }

        [SerializeField] public CollectibleObject.ObjectType CollectObjTargetType;

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

        private int _currentAmount;

        public int RequiredAmount
        {
            get => _requiredAmount;
        }

        [SerializeField] private int _requiredAmount;

        protected void Awake()
        {
            MakeDescription();
            Debug.Log(GoalDescription);
        }

        // Start is called before the first frame update
        protected void Start()
        {
            LevelInfo.Instance.OnCollectibleObjectDie += CollectibleObjectDied_GoalCollectHandler;
            UI_QuestInfoController.Instance.InitGoal(this);
        }

        private void CollectibleObjectDied_GoalCollectHandler(CollectibleObject.ObjectType type)
        {
            CurrentAmount = LevelInfo.Instance.CollectibleTypesCounter[type];
            OnProgressChanged?.Invoke(CurrentAmount);
        }

        private void MakeDescription()
        {
            GoalDescription += $"Collect {_requiredAmount} ";
            switch (CollectObjTargetType)
            {
                case CollectibleObject.ObjectType.RusMailBox:
                    GoalDescription += $"lost Russian Post packages.";
                    break;
                case CollectibleObject.ObjectType.AmazMailBox:
                    GoalDescription += $"Amazon packages.";
                    break;
                default:
                    throw new InvalidEnumArgumentException($"MakeDescription(): CollectObjTargetType - {CollectObjTargetType.ToString()}");
            }
        }
    }
}