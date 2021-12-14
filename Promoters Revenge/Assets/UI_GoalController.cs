using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Quest;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UI_GoalController : MonoBehaviour
    {
        [SerializeField] private Image _goalIcon;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _progress;
        [SerializeField] private Animator _anim;
        [SerializeField] private int _requiredAmount;


        public void InitGoal(GoalDefeat goalDefeat)
        {
            _goalIcon.sprite = goalDefeat.GoalIcon;
            _description.text = goalDefeat.GoalDescription;
            _requiredAmount = goalDefeat.RequiredAmount;
            _progress.text = $"0/{_requiredAmount}";
            goalDefeat.OnComplete += AnimationOnComplete;
            goalDefeat.OnProgressChanged += ChangeProgress;
        }

        public void InitGoal(GoalCollect goalCollect)
        {
            _goalIcon.sprite = goalCollect.GoalIcon;
            _description.text = goalCollect.GoalDescription;
            _requiredAmount = goalCollect.RequiredAmount;
            _progress.text = $"0/{_requiredAmount}";
            goalCollect.OnComplete += AnimationOnComplete;
            goalCollect.OnProgressChanged += ChangeProgress;
        }

        public void ChangeProgress(int currentAmount)
        {
            _progress.text = $"{currentAmount}/{_requiredAmount}";
        }

        public void AnimationOnComplete()
        {
            _anim.SetTrigger("GoalCompleted");
            transform.SetAsLastSibling();
        }
    }
}