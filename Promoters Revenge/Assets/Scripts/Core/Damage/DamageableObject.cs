using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Damage;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.UI.ProgressBars;

namespace Assets.Scripts.Core.Damage
{
    public class DamageableObject : MonoBehaviour, IDamageable
    {
        public enum HitEffect
        {
            None,
            ScaleDown,
            ScaleUp
        }

        [SerializeField] private HitEffect _hitEffect;

        public int CurrHealth
        {
            get => _currHealth;
            set => _currHealth = Mathf.Clamp(value, 0, _maxHealth);
        }

        [SerializeField] private int _currHealth = 0;

        public int MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = Mathf.Max(1, value);
        }

        [SerializeField] private int _maxHealth = 1;
        [SerializeField] private float _dmgMultiplier = 1f;
        [SerializeField] private float _healMultiplier = 1f;

        [SerializeField] private bool _isImmortal = false;
        [SerializeField] private bool _isInvulnerable = false;

        private ProgressBar _hpBar;

        protected void Awake()
        {
            _hpBar = GetComponentInChildren<ProgressBar>();
        }

        protected void Start()
        {
            //Debug.LogWarning(_hpBar.BarValue + " wtf");
        }

        protected void OnEnable()
        {
            ResetDamageableObject();
        }

        private void ResetDamageableObject()
        {
            CurrHealth = MaxHealth;
            //UpdateLocalUI(); //NullReference :(((
        }


        public bool TakeHeal(Damage heal)
        {
            CurrHealth += (int) Mathf.Round(heal.Amount * _healMultiplier);
            UpdateLocalUI();
            return ApplyDamage(heal);
        }

        public bool TakeDamage(Damage dmg)
        {
            HitEffectLogic();
            if (_isInvulnerable)
            {
                return ApplyDamage(dmg);
            }

            var totalDmg = (int) Mathf.Round(dmg.Amount * _dmgMultiplier);
            CurrHealth -= totalDmg;
            UpdateLocalUI();
            CheckHealth();

            return ApplyDamage(dmg);
        }

        private void CheckHealth()
        {
            if (CurrHealth == 0)
            {
                Die();
            }
        }

        private bool ApplyDamage(Damage dmg)
        {
            dmg.IsApplied = true;
            //Just for test
            return true;
        }

        private void Die()
        {
            if (_isImmortal)
            {
                return;
            }

            Debug.Log($"{gameObject.name} died :(");
            gameObject.SetActive(false);
        }

        private void HitEffectLogic()
        {
            switch (_hitEffect)
            {
                case HitEffect.None:
                    return;
                case HitEffect.ScaleDown:
                    return;
                case HitEffect.ScaleUp:
                    return;
                default:
                    Debug.LogWarning($"{_hitEffect} - Default Switch on _hitEffect???");
                    return;
            }
        }

        private void UpdateLocalUI()
        {
            _hpBar.ChangeValue((float) _currHealth / _maxHealth);

            //Debug.LogWarning(_hpBar.BarValue + " - hp BAR || Curr= " + _currHealth / _maxHealth);
        }

        public event EventHandler OnHealApplied;
        public event EventHandler OnDamageApplied;
        public event EventHandler OnDied;
    }
}