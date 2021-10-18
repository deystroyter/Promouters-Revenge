using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Quest
{
    public interface IGoal
    {
        public delegate void GoalCompleted();
        public event GoalCompleted OnComplete;

        public string GoalDescription { get; }
        public int CurrentAmount { get; }
    }
}