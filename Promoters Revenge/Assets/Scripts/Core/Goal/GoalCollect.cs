using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Core.Collect;

namespace Assets.Scripts.Core.Quest
{
    public class GoalCollect : MonoBehaviour, IGoal
    {
        private LevelManager _levelManager;

        public event IGoal.GoalCompleted OnComplete;

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
                }
            }
        }
        private int _currentAmount;
        [SerializeField] private int _requiredAmount;

        protected void Awake()
        {
            MakeDescription();
            Debug.Log(GoalDescription);
        }

        // Start is called before the first frame update
        protected void Start()
        {
            _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            _levelManager.OnCollectibleObjectDie += CollectibleObjectDied_GoalCollectHandler;
        }

        private void CollectibleObjectDied_GoalCollectHandler(CollectibleObject.ObjectType type)
        {
            CurrentAmount = _levelManager.CollectibleTypesCounter[type];
        }

        private void MakeDescription()
        {
            GoalDescription += $"Соберите {_requiredAmount} ";
            switch (CollectObjTargetType)
            {
                case CollectibleObject.ObjectType.RusMailBox:
                    GoalDescription += $"потерянных посылок Почты Росси.";
                    break;
                case CollectibleObject.ObjectType.AmazMailBox:
                    GoalDescription += $"интернет-посылок.";
                    break;
                default:
                    GoalDescription += $"UKNOWN COLLECTIBLE OBJECT :((";
                    break;
            }
        }
    }
}