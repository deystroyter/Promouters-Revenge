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
        [SerializeField] private float _bulletLifeTime = 5f;

        [SerializeField] private ushort _collisionsLife = 5;

        [SerializeField] [Range(2, 100)] private ushort _fadeStepsCount = 20;

        [SerializeField] [Range(0.01f, 0.99f)] private float _percentOfLifeFade = 0.60f;


        private float _curBulletLifeTime; // сколько сейчас осталось снаряду
        private ushort _curCollisions; // сколько раз столкнулся снаряд

        private float _fadeStep;
        private bool _isFading = false;

        private Renderer _bulletRenderer;

        //Coroutine for smooth fade and hide object
        private IEnumerator SmoothFade()
        {
            for (float f = 1f; f >= 0; f -= _fadeStep)
            {
                Color c = _bulletRenderer.material.color;
                c.a = f > 0 ? f : 0;
                _bulletRenderer.material.color = c;

                if (f < _fadeStep)
                {
                    _isFading = false;
                    DestroyObject();
                    ResetBullet();
                }

                yield return new WaitForSeconds(_fadeStep);
            }
        }

        // Start is called before the first frame update
        protected void Start()
        {
            _fadeStep = _bulletLifeTime * (1 - _percentOfLifeFade) / _fadeStepsCount;
            _bulletRenderer = GetComponentInChildren<Renderer>();
        }


        private void ResetBullet()
        {
            _curBulletLifeTime = _bulletLifeTime;
            _curCollisions = 0;

            Color c = _bulletRenderer.material.color;
            c.a = 1f;
            _bulletRenderer.material.color = c;
        }

        // Update is called once per frame
        protected void Update()
        {
            if (_curBulletLifeTime <= _bulletLifeTime * (1 - _percentOfLifeFade) && !_isFading)
            {
                _isFading = true;
                StartCoroutine("SmoothFade", _bulletLifeTime * (1 - _percentOfLifeFade));
            }

            _curBulletLifeTime -= Time.deltaTime;
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out DamageableObject dmgObj))
            {
                if (dmgObj.TakeDamage(new Damage(_dmgAmount, gameObject)))
                {
                    DestroyObject();
                }

                ;
            }
        }

        protected void OnCollisionExit(Collision collision)
        {
            if (_curCollisions >= _collisionsLife)
            {
                DestroyObject();
            }
            else
            {
                _curCollisions++;
            }
        }

        public void DestroyObject()
        {
            ObjectPooler.ObjPooler.DestroyObject(gameObject);
        }
    }
}