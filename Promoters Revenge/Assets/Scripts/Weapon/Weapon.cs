using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour
    {
        //public static Weapon _instance { get; private set; }

        [SerializeField] private ObjectPooler.ObjectInfo.ObjectType bulletType;

        [Header("Magazine And Ammo")]
        [SerializeField] private uint _ammoCount = 200; //всего патронов

        [SerializeField] private uint _currAmmo = 20; //патронов в магазине
        [SerializeField] private const uint MagSize = 20; //размер обоймы
        [SerializeField] private const float ShootSpeed = 0.17f; //скорострельность (время на выстрел)
        [SerializeField] private const float ReloadSpeed = 1f; //скорость перезарядки (Время на перезарядку)

        [Header("Audio")]
        [SerializeField] private AudioClip _fireAudioClip;

        [SerializeField] private AudioClip _reloadAudioClip;

        private float reloadTimer = 0.0f; //(не трогать)
        private float shootTimer = 0.0f; //(не трогать)


        private Transform gunpoint; //точка выстрела (дуло)
        private AudioSource audioSrc;

        public delegate void AmmoChangeEvent(uint currAmmo, uint ammoCount);

        public event AmmoChangeEvent OnAmmoChanged;

        protected void Awake()
        {
            //_instance = this;
        }


        protected void Start()
        {
            audioSrc = GetComponent<AudioSource>();
            // LoadAudioClips();
            if (gunpoint == null)
            {
                foreach (Transform child in gameObject.transform)
                {
                    if (child.name == "gunpoint")
                        gunpoint = child.transform;
                }
            }

            // OnAmmoChanged?.Invoke(_currAmmo, _ammoCount);
        }

        protected void Update()
        {
            ShootLogic();
            ReloadLogic();
        }

        private void ShootLogic()
        {
            if (Input.GetMouseButton(0) & _currAmmo > 0 & reloadTimer <= 0 & shootTimer <= 0)
            {
                GameObject bullet = ObjectPooler.ObjPooler.GetObject(bulletType);
                if (bullet == null) return;
                bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
                bullet.transform.position = gunpoint.position;
                bullet.transform.rotation = gunpoint.rotation;
                bullet.SetActive(true);
                bullet.GetComponent<Rigidbody>().AddForce(gunpoint.transform.forward * 5000);

                PlayAudioOneTime(_fireAudioClip);

                _currAmmo = _currAmmo - 1;

                OnAmmoChanged?.Invoke(_currAmmo, _ammoCount);

                if (_currAmmo == 0)
                {
                    ReloadLogic();
                }

                shootTimer = ShootSpeed;
            }


            if (shootTimer > 0) shootTimer -= Time.deltaTime;
        }

        private void ReloadLogic()
        {
            if (Input.GetKeyDown(KeyCode.R) && _currAmmo < MagSize)
            {
                reloadTimer = ReloadSpeed;
                _currAmmo = _ammoCount / MagSize;

                PlayAudioOneTime(_reloadAudioClip);

                OnAmmoChanged?.Invoke(_currAmmo, _ammoCount);

                if (shootTimer > 0) shootTimer -= Time.deltaTime;
            }

            if (reloadTimer > 0) reloadTimer -= Time.deltaTime;
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