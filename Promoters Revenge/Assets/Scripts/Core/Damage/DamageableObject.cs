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

        public enum ObjectType
        {
            Enemy,
            TestCube
        }

        [SerializeField] public ObjectType ObjType;

        public int Health
        {
            get => _health;
            set
            {
                if (_isInvulnerable) return;

                _health = Mathf.Clamp(value, 0, _maxHealth);

                UpdateLocalUI();
                if (!_isImmortal && Health == 0)
                {
                    Die();
                }
            }
        }

        [SerializeField] private int _health = 0;

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

        protected void OnEnable()
        {
            ResetDamageableObject();
        }

        private void ResetDamageableObject()
        {
            Health = MaxHealth;
        }


        public bool TakeHeal(Damage heal)
        {
            Health += (int) Mathf.Round(heal.Amount * _healMultiplier);


            OnHealApplied?.Invoke(heal.Amount);
            return ApplyDamage(heal);
        }

        public bool TakeDamage(Damage dmg)
        {
            HitEffectLogic();

            var totalDmg = (int) Mathf.Round(dmg.Amount * _dmgMultiplier);
            Health -= totalDmg;


            return ApplyDamage(dmg);
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

            LevelInfo.Instance.DamageableObjectDied(ObjType);

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
                    transform.localScale = Vector3.one * 0.9f;
                    return;
                case HitEffect.ScaleUp:
                    return;
                default:
                    Debug.LogWarning($"{_hitEffect} - Default Switch on _hitEffect???");
                    return;
            }
        }

        private IEnumerable ScaleToNormal()
        {
            while (transform.localScale != Vector3.one)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.5f);
            }

            yield break;
        }


        private void UpdateLocalUI()
        {
            _hpBar.ChangeValue((float) _health / _maxHealth);

            //Debug.LogWarning(_hpBar.BarValue + " - hp BAR || Curr= " + _health / _maxHealth);
        }

        public delegate void HealthChange(int amount);

        public event HealthChange OnHealApplied;
        public event HealthChange OnDamageApplied;
    }
}