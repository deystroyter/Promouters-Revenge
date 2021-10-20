using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.Scripts.Core.Damage;
using UnityEngine;

namespace Assets.Scripts.Core.Collect
{
    public class CollectibleObject : MonoBehaviour
    {
        public enum ObjectType
        {
            RusMailBox,
            AmazMailBox
        }

        [SerializeField] public ObjectType ObjType;

        public enum IdleMove
        {
            None,
            Rotating,
            Bouncing
        }

        [SerializeField] public IdleMove Idle;

        public enum CollectEffect
        {
            None,
            FlyUp,
            Fade,
            Break
        }

        [SerializeField] private CollectEffect _collectEffect;


        // Start is called before the first frame update
        // Update is called once per frame
        protected void Update()
        {
            IdleLogic();
        }

        private void IdleLogic()
        {
            switch (Idle)
            {
                case IdleMove.None:
                    return;
                case IdleMove.Rotating:
                    transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
                    return;
                default:
                    throw new InvalidEnumArgumentException($"IdleLogic(): IdleMove - {Idle.ToString()}");
            }
        }

        private void OnCollectLogic()
        {
            gameObject.SetActive(false);
            switch (_collectEffect)
            {
                case CollectEffect.None:
                    gameObject.SetActive(false);
                    return;
                case CollectEffect.FlyUp:
                    return;
                default:
                    throw new InvalidEnumArgumentException($"OnCollectLogic(): _collectEffect - {_collectEffect.ToString()}");
            }
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                OnCollectLogic();
                Die();
            }
        }

        private void Die()
        {
            LevelInfo.Instance.CollectibleObjectDied(ObjType);
        }
    }
}