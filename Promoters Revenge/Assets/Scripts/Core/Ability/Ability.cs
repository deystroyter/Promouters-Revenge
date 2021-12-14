using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Ability
{
    public abstract class Ability : MonoBehaviour
    {
        [Header("Ability Common Settings")] public Sprite AbilityIcon;
        public string AbilityInput;

        public float Cooldown;
        private float _cooldown;
        public bool isAvailable;
    }
}