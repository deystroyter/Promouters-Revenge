using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts.UI.HUD;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        public Sprite WeaponIconSprite;
        [SerializeField] private ObjectPooler.ObjectInfo.ObjectType bulletType;

        public int AmmoAmount
        {
            get => _ammoAmount;
            set => _ammoAmount = Mathf.Clamp(value, 0, _maxAmmo);
        }
        public int AmmoInMag
        {
            get => _ammoInMag;
            set => _ammoInMag = Mathf.Clamp(value, 0, MagSize);
        }

        [Header("Magazine And Ammo")]
        [SerializeField] private int _ammoAmount = 200; //всего патронов

        [SerializeField] private int _maxAmmo = 300;
        [SerializeField] private int _ammoInMag = 20; //патронов в магазине
        [SerializeField] private const int MagSize = 20; //размер обоймы

        [Header("Shoot & Reaload Speed")]
        [SerializeField] private const float ShootSpeed = 0.17f; //скорострельность (время на выстрел)

        [SerializeField] private const float ReloadSpeed = 1f; //скорость перезарядки (Время на перезарядку)

        [Header("Audio")]
        [SerializeField] private AudioClip _fireAudioClip;

        [SerializeField] private AudioClip _reloadAudioClip;

        private float reloadTimer = 0.0f; //(не трогать)
        private float shootTimer = 0.0f; //(не трогать)


        private Transform gunpoint; //точка выстрела (дуло)
        private AudioSource audioSrc;


        protected void Start()
        {
            audioSrc = GetComponent<AudioSource>();
            if (gunpoint == null)
            {
                foreach (Transform child in gameObject.transform)
                {
                    if (child.name == "gunpoint")
                    {
                        gunpoint = child.transform;
                    }
                }

                if (gunpoint == null)
                {
                    throw new Exception("No founded gunpoint in Weapon!");
                }
            }
        }

        protected void Update()
        {
            ShootLogic();
            ReloadLogic();
        }

        private void ShootLogic()
        {
            if (Input.GetMouseButton(0) && _ammoInMag > 0 && reloadTimer <= 0 && shootTimer <= 0)
            {
                GameObject bullet = ObjectPooler.ObjPooler.GetObject(bulletType);
                if (bullet == null)
                {
                    return;
                }

                bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
                bullet.transform.position = gunpoint.position;
                bullet.transform.rotation = gunpoint.rotation;
                bullet.SetActive(true);
                bullet.GetComponent<Rigidbody>().AddForce(gunpoint.transform.forward * 5000);

                PlayAudioOneTime(_fireAudioClip);

                _ammoInMag--;

                HUDManager.Instance.UpdateGunAmmoInfo(AmmoInMag, AmmoAmount);

                if (_ammoInMag == 0)
                {
                    ReloadLogic();
                }

                shootTimer = ShootSpeed;
            }


            if (shootTimer > 0) shootTimer -= Time.deltaTime;
        }

        private void ReloadLogic()
        {
            if (Input.GetKeyDown(KeyCode.R) && _ammoInMag < MagSize)
            {
                reloadTimer = ReloadSpeed;
                _ammoInMag = _ammoAmount / MagSize;

                PlayAudioOneTime(_reloadAudioClip);

                HUDManager.Instance.UpdateGunAmmoInfo(AmmoInMag, AmmoAmount);

                if (shootTimer > 0)
                {
                    shootTimer -= Time.deltaTime;
                }
            }

            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
            }
        }

        private void PlayAudioOneTime(AudioClip clip)
        {
            audioSrc.PlayOneShot(clip);
        }

        private void LoadAudioClips()
        {
            _fireAudioClip = Resources.Load<AudioClip>("Audio/Weapons/" + gameObject.name + "fire.mp3");
            _reloadAudioClip = Resources.Load<AudioClip>("Audio/Weapons/" + gameObject.name + "reload.mp3");
        }
    }
}