using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Ability;
using UnityEngine;

namespace Assets.Scripts.Core.Ability
{
    public class ThrowingAbility : Ability
    {
        [Header("Throwing Ability")] public ThrowingSystem ThrowingSystem;
        public GameObject ProjectilePrefab;
        public float MaxThrowRadius;
        public float FlightTime;

        public event Action OnUse;

        // Start is called before the first frame update
        void Start()
        {
            ThrowingSystem = GameObject.FindGameObjectWithTag("ThrowingSystem").GetComponent<ThrowingSystem>();
        }

        // Update is called once per frame
        protected void Update()
        {
            if (GameInput.Key.GetKey(AbilityInput))
            {
                ThrowingSystem.ThrowingInput = AbilityInput;
                ThrowingSystem.maxThrowRadius = MaxThrowRadius;
                ThrowingSystem.ProjectilePrefab = ProjectilePrefab;
                ThrowingSystem.flightTime = FlightTime;
                if (Input.GetMouseButtonDown(0))
                {
                    OnUse?.Invoke();
                }
            }
        }
    }
}