using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Damage
{
    public interface IDamageable
    {
        public int Health { get; }
        public int MaxHealth { get; }

        public bool TakeDamage(Damage dmg);
        public bool TakeHeal(Damage dmg);
    }
}