using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Ability;
using UnityEngine;

public class AbilitiesSystem : MonoBehaviour
{
    public bool AutoInput = true;

    protected void Start()
    {
        if (!AutoInput)
        {
            return;
        }

        var i = 1;

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<ThrowingAbility>(out var abilityScript) && i <= transform.childCount)
            {
                abilityScript.AbilityInput = "Ability0" + i;
                i++;
            }
            else
            {
                throw new System.Exception("Abilities System Error - Unknown Ability Type");
            }
        }
    }
}