using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Core.Damage;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class Enemy : MonoBehaviour
    {
        public GameObject Target;
        public GameObject Player;
        private Rigidbody _rgBody;
        private Animator _anim;
        private TargetFollower _targetFollower;

        [SerializeField] private float AttackDistance = 1.5f;
        [SerializeField] private float DamageAmount = 15f;
        [SerializeField] private float DamageDelayTime = 1f;
        [SerializeField] private float AttackCooldownTime = 2f;

        private bool _isAttacking = false;

        // Start is called before the first frame update
        protected void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            Target = Player;
            _targetFollower = GetComponent<TargetFollower>();
            _rgBody = GetComponent<Rigidbody>();
            _anim = GetComponent<Animator>();
            _anim.SetTrigger("Run");
        }

        // Update is called once per frame
        protected void Update()
        {
            if (!_isAttacking && (Vector3.Distance(transform.position, Target.transform.position) < AttackDistance))
            {
                Attack();
            }

            if (!Target.activeSelf)
            {
                Target = Player;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == Target)
            {
                Attack();
            }
        }

        private void Attack()
        {
            _isAttacking = true;
            if (Random.Range(0.01f, 1f) >= 0.50f)
            {
                _anim.SetTrigger("AttackLeg");
            }
            else
            {
                _anim.SetTrigger("AttackPunch");
            }

            _targetFollower.PauseFollow();
            StartCoroutine(DamageDelayed());
        }

        private IEnumerator DamageDelayed()
        {
            yield return new WaitForSeconds(DamageDelayTime);
            if (Vector3.Distance(transform.position, Target.transform.position) < AttackDistance * 2f)
            {
                Target.TryGetComponent<DamageableObject>(out var dmgObj);
                dmgObj.TakeDamage(new Damage(5f, gameObject));
            }

            StartCoroutine(AttackResetWithCooldown());
            yield break;
        }

        private IEnumerator AttackResetWithCooldown()
        {
            yield return new WaitForSeconds(AttackCooldownTime);
            _isAttacking = false;
            _targetFollower.ContinueFollow();
            _anim.SetTrigger("Run");
            yield break;
        }
    }
}