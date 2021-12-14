using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Ability;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UI_AbilitiesPanel : MonoBehaviour
    {
        public AbilitiesSystem AbilitiesSystem;
        public GameObject AbilityPrefab;

        // Start is called before the first frame update
        protected void Start()
        {
            StartCoroutine(InitAbilitiesPanel());
        }

        public void UpdateUIAbilitiesPanel()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            StartCoroutine(InitAbilitiesPanel());
        }

        public IEnumerator InitAbilitiesPanel()
        {
            yield return new WaitForSeconds(0.5f);
            foreach (Transform child in AbilitiesSystem.transform)
            {
                if (child.TryGetComponent<ThrowingAbility>(out var ability))
                {
                    var UIAbilityGO = Instantiate(AbilityPrefab, transform);

                    var UI_Ability = UIAbilityGO.GetComponent<UI_Ability>();
                    ability.OnUse += UI_Ability.DrawCooldown;
                    //UI_Ability.Cooldown = ability.Cooldown;
                    UI_Ability.SetIcon(ability.AbilityIcon);

                    UI_Ability.SetKeyText("" + GameInput.Key.FindKey(ability.AbilityInput));
                }
            }

            yield break;
        }
    }
}