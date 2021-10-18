using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Assets.Scripts.Weapon;

namespace Assets.Scripts.UI.HUD
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private GameObject Player;
        [SerializeField] private GameObject _weapon;

        [SerializeField] private TextMeshProUGUI _ammoText;


        [SerializeField] private GameObject HpInfoHUD;
        [SerializeField] private GameObject GoalsInfoHUD;

        protected void Start()
        {
            _weapon.GetComponent<WeaponController>().OnAmmoChanged += UpdateGunInfoHUD;
        }


        private void UpdateGunInfoHUD(int currAmmo, int ammoCount)
        {
            _ammoText.SetText($"{currAmmo}/{ammoCount}");
        }

        private void UpdateGoalsInfoHUD()
        {
        }

        private void UpdateHpInfoHUD()
        {
        }
    }
}