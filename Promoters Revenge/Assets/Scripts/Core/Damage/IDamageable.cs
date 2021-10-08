using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Damage
{
    public interface IDamageable
    {
        public int CurrHealth { get; set; }
        public int MaxHealth { get; set; }

        public bool TakeDamage(Damage dmg);
        public bool TakeHeal(Damage dmg);

        public event EventHandler OnHealApplied;
        public event EventHandler OnDamageApplied;
        public event EventHandler OnDied;
    }
}