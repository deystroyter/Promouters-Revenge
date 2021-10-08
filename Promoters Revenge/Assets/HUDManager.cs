using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Weapon;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject _weapon;

    [SerializeField] private GameObject AmmoText;
    //[SerializeField] private TextMeshPro 


    [SerializeField] private GameObject HpInfoHUD;
    [SerializeField] private GameObject TasksInfoHUD;

    protected void Awake()
    {
        //AmmoText = GetComponent<TextMeshPro>();
        //AmmoText = GameObject.FindGameObjectWithTag("Test");
    }

    protected void Start()
    {
        _weapon.GetComponent<Weapon>().OnAmmoChanged += UpdateGunInfoHUD;
    }

    protected void Update()
    {
    }


    private void UpdateGunInfoHUD(uint currAmmo, uint ammoCount)
    {
        Debug.Log("Eaesae");
        AmmoText.GetComponent<TextMeshProUGUI>().SetText($"{currAmmo}/{ammoCount}");
    }

    private void UpdateHpInfoHUD()
    {
    }

    private void UpdateTasksInfoHUD()
    {
    }
}