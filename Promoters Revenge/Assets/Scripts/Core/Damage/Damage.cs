using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Damage
{
    public class Damage
    {
        public int Amount { get; private set; }

        public enum DamageContext
        {
            UNKNOWN,
            COMBAT,
            MAP,
            NPC
        }

        public Damage(float amount, GameObject dmgSourse)
        {
            Amount = (int) Mathf.Round(amount);
            DmgSourse = dmgSourse;
        }

        public bool IsApplied { get; set; }

        public GameObject DmgSourse { get; private set; }
        public GameObject SourceAbility;
    }
}