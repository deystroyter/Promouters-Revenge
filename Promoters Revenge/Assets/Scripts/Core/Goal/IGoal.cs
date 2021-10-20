using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Quest
{
    public interface IGoal
    {
        public event Action OnComplete;

        public string GoalDescription { get; }
        public int CurrentAmount { get; }
    }
}