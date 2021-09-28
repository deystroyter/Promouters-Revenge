using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private ObjectPooler.ObjectInfo.ObjectType bulletType;

    public int AmmoCount = 200; //всего патронов
    public int CurAmmo = 20; //патронов в магазине
    public int MagSize = 20; //размер обоймы
    public AudioClip Fire;

    public float ShootSpeed = 0.17f; //скорострельность (время на выстрел)
    public float ReloadSpeed = 1f; //скорость перезарядки (Время на перезарядку)
    public AudioClip Reload;

    private float ReloadTimer = 0.0f; //(не трогать)
    private float ShootTimer = 0.0f; //(не трогать)

    //public Transform bullet; //GameObject патрона
    public Transform gunpoint; //точка выстрела

    private void Update()
    {
        ShootLogic();
        ReloadLogic();
    }

    private void ShootLogic()
    {
        //TODO: Object pooling
        if (Input.GetMouseButton(0) & CurAmmo > 0 & ReloadTimer <= 0 & ShootTimer <= 0)
        {
            GameObject bullet = ObjectPooler.current.GetObject(bulletType);
            if (bullet == null) return;
            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bullet.transform.position = gunpoint.position;
            bullet.transform.rotation = gunpoint.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody>().AddForce(gunpoint.transform.forward * 5000);
            // Transform BulletInstance = Instantiate(bullet, gunpoint.position, Quaternion.Lerp(bullet.rotation, gunpoint.rotation, 1f));
            // BulletInstance.GetComponent<Rigidbody>().AddForce(gunpoint.transform.forward * 5000);
            CurAmmo = CurAmmo - 1;
            if (CurAmmo == 0) ReloadLogic();
            //audio.PlayOneShot(Fire);
            ShootTimer = ShootSpeed;
        }
        if (ShootTimer > 0) ShootTimer -= Time.deltaTime;
    }

    private void ReloadLogic()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadTimer = ReloadSpeed;
            CurAmmo = MagSize;
            //audio.PlayOneShot(Reload);

            if (ShootTimer > 0) ShootTimer -= Time.deltaTime;
        }

        if (ReloadTimer > 0) ReloadTimer -= Time.deltaTime;
    }
}