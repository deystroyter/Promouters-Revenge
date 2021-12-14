using System;
using System.Collections;
using System.ComponentModel;
using Assets.Scripts.Core.Damage;
using UnityEngine;


namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Grenade : MonoBehaviour
    {
        public enum GrenadeType
        {
            Explosive,
            Distraction,
            Disorienting
        }

        public GrenadeType GrenType;

        public bool IsStandingOnBottom;
        public bool IsSticky;

        public float DamageAmount = 10f;
        public float TimeToAct = 2f;
        public float BlastRadius = 5f;

        private bool _isCollided;
        private Rigidbody _rigidbody;

        protected void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected void Update()
        {
            if (_isCollided)
            {
                StandingOnBottomLogic();
            }
        }

        private void StandingOnBottomLogic()
        {
            if (IsStandingOnBottom)
            {
                if (transform.position.y <= 1f && transform.rotation != Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0))
                {
                    transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Time.deltaTime);
                }
            }
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                return;
            }

            _rigidbody.freezeRotation = true;
            _rigidbody.velocity = Vector3.up * _rigidbody.velocity.y;

            _isCollided = true;
            if (IsSticky)
            {
                transform.parent = collision.gameObject.transform;
            }


            switch (GrenType)
            {
                case GrenadeType.Explosive:
                    Explode();
                    break;
                case GrenadeType.Distraction:
                    Distract();
                    break;
                case GrenadeType.Disorienting:
                    Disorient();
                    break;
                default:
                    throw new InvalidEnumArgumentException($"Unknown GrenadeType. || {gameObject.name}");
            }
        }

        private void Explode()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, BlastRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.TryGetComponent(out DamageableObject dmgObj))
                {
                    dmgObj.TakeDamage(new Damage(DamageAmount, gameObject));
                }
            }

            Destroy(gameObject);
        }

        private void Distract()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, BlastRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.TryGetComponent(out TargetFollower targetFollower))
                {
                    targetFollower.FollowForSeconds(gameObject, TimeToAct);
                }
            }

            StartCoroutine(DestroyWithDelay());
        }

        public IEnumerator DestroyWithDelay()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
            yield break;
        }

        //TODO: Think about Disorientation
        private void Disorient()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, BlastRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.TryGetComponent(out TargetFollower targetFollower))
                {
                    targetFollower.FollowForSeconds(gameObject, TimeToAct);
                }
            }
        }
    }
}