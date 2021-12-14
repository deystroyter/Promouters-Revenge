using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Quest;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UI_QuestInfoController : MonoBehaviour
    {
        public static UI_QuestInfoController Instance;
        public GameObject QuestPrefab;

        protected void Awake()
        {
            Instance = this;
        }

        public void InitGoal(GoalDefeat goalDefeat)
        {
            var UIQuestGO = Instantiate(QuestPrefab, transform);
            if (UIQuestGO.TryGetComponent<UI_GoalController>(out var goalController))
            {
                goalController.InitGoal(goalDefeat);
            }
        }

        public void InitGoal(GoalCollect goalCollect)
        {
            var UIQuestGO = Instantiate(QuestPrefab, transform);
            if (UIQuestGO.TryGetComponent<UI_GoalController>(out var goalController))
            {
                goalController.InitGoal(goalCollect);
            }
        }
    }
}