using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Assets.Scripts.Weapon;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance;

        [SerializeField] private TextMeshProUGUI _ammoText;
        [SerializeField] private Image _gunIconImage;
        [SerializeField] private TextMeshProUGUI _gunNameText;


        [SerializeField] private GameObject HpInfoHUD;
        [SerializeField] private GameObject GoalsInfoHUD;

        protected void Awake()
        {
            Instance = this;
        }

        public void UpdateNewGunInfo(Sprite gunIcon, string gunName, int currAmmo, int ammoCount)
        {
            _gunIconImage.overrideSprite = gunIcon;
            _gunNameText.text = gunName;
            UpdateGunAmmoInfo(currAmmo, ammoCount);
        }

        public void UpdateGunAmmoInfo(int currAmmo, int ammoCount)
        {
            _ammoText.SetText($"{currAmmo}/{ammoCount}");
        }

        public void UpdateGoalsInfoHUD()
        {
        }

        public void UpdatePlayerHpInfoHUD()
        {
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}