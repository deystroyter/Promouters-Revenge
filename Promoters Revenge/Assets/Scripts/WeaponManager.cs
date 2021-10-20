using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.HUD;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class WeaponManager : MonoBehaviour
    {
        private WeaponController _activeWeaponController;

        // Start is called before the first frame update
        protected void Start()
        {
            _activeWeaponController = GetComponentInChildren<WeaponController>();
            HUDManager.Instance.UpdateNewGunInfo(_activeWeaponController.WeaponIconSprite, _activeWeaponController.gameObject.name, _activeWeaponController.AmmoInMag, _activeWeaponController.AmmoAmount);
        }

        public void ChangeWeapon()
        {
        }
    }
}