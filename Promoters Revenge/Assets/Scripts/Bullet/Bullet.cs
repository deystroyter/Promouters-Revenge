using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Damage;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour, IPooledObject
    {
        public ObjectPooler.ObjectInfo.ObjectType Type => _bulletType;

        [SerializeField] private ObjectPooler.ObjectInfo.ObjectType _bulletType;


        [SerializeField] private float _dmgAmount = 10f;


        [Header("Visual Effects")]
        [SerializeField] private float _maxLifeTime = 5f;

        [SerializeField] private ushort _maxCollisions = 5;

        [SerializeField] [Range(2, 100)] private ushort _fadeStepsCount = 20;

        [SerializeField] [Range(0.01f, 0.99f)] private float _percentOfLifeFade = 0.60f;


        private float _currentLifeTime; // сколько сейчас осталось снаряду
        private ushort _currentCollisions; // сколько раз столкнулся снаряд

        private bool _isFading = false;

        private Renderer _bulletRenderer;

        //Coroutine for smooth fade and hide object
        private IEnumerator SmoothFade(float currentLifeTime)
        {
            var fadeStepTime = (_maxLifeTime - currentLifeTime) / _fadeStepsCount;

            for (float f = 1f; f >= 0; f -= fadeStepTime)
            {
                Color c = _bulletRenderer.material.color;
                c.a = f > 0 ? f : 0;
                _bulletRenderer.material.color = c;

                if (f < fadeStepTime)
                {
                    _isFading = false;
                    DestroyObject();
                    ResetBullet();
                }

                yield return new WaitForSeconds(fadeStepTime);
            }
        }

        // Start is called before the first frame update
        protected void Start()
        {
            _bulletRenderer = GetComponentInChildren<Renderer>();
        }


        private void ResetBullet()
        {
            _currentLifeTime = _maxLifeTime;
            _currentCollisions = 0;

            Color c = _bulletRenderer.material.color;
            c.a = 1f;
            _bulletRenderer.material.color = c;
        }

        // Update is called once per frame
        protected void Update()
        {
            if (_currentLifeTime <= _maxLifeTime * _percentOfLifeFade && !_isFading)
            {
                _isFading = true;
                StartCoroutine(SmoothFade(_currentLifeTime - Time.deltaTime));
            }

            _currentLifeTime -= Time.deltaTime;
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out DamageableObject dmgObj))
            {
                if (dmgObj.TakeDamage(new Damage(_dmgAmount, gameObject)))
                {
                    DestroyObject();
                }
            }
        }

        protected void OnCollisionExit(Collision collision)
        {
            if (_currentCollisions >= _maxCollisions)
            {
                DestroyObject();
            }
            else
            {
                _currentCollisions++;
            }
        }

        public void DestroyObject()
        {
            ObjectPooler.ObjPooler.DestroyObject(gameObject);
        }
    }
}