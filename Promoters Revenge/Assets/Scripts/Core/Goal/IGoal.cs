using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Quest
{
    public interface IGoal
    {
        public event Action OnComplete;
        public event Action<int> OnProgressChanged;

        public string GoalDescription { get; }
        public int CurrentAmount { get; }
        public int RequiredAmount { get; }
    }
}