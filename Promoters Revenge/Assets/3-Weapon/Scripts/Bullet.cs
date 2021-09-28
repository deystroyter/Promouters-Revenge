using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    public ObjectPooler.ObjectInfo.ObjectType Type => type;

    [SerializeField]
    private ObjectPooler.ObjectInfo.ObjectType type;

    public float BulletLifeTime = 5f; //сколько секунд на экране объект снаряда
    public ushort CollisionsLife = 5; //сколько коллизий переживёт снаряд

    private float curBulletLifeTime; //сколько сейчас осталось снаряду
    private ushort curCollisions = 0; //сколько раз столкнулся снаряд

    [Range(0.001f, 0.999f)]
    public float PercentOfLifeFade = 0.60f;

    [Range(1, 100)]
    public ushort FadeSteps = 20; //за сколько шагов объект полностью исчезнет

    private float fadeStep;
    private bool isFading = false;

    private Renderer bulletRenderer;

    //Coroutine for smooth fade and hide object
    private IEnumerator Fade()
    {
        for (float f = 1f; f >= 0; f -= fadeStep)
        {
            Color c = bulletRenderer.material.color;
            c.a = f > 0 ? f : 0;
            bulletRenderer.material.color = c;

            if (f < fadeStep)
            {
                isFading = false;
                ObjectPooler.current.DestroyObject(gameObject);
            }

            yield return new WaitForSeconds(fadeStep);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        fadeStep = BulletLifeTime * (1 - PercentOfLifeFade) / FadeSteps;
        bulletRenderer = this.GetComponentInChildren<Renderer>();
    }

    private void OnEnable()
    {
        ResetBullet();
    }

    private void ResetBullet()
    {
        Color c = bulletRenderer.material.color;
        c.a = 1f;
        bulletRenderer.material.color = c;
        curCollisions = 0;
        curBulletLifeTime = BulletLifeTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (curBulletLifeTime <= BulletLifeTime * (1 - PercentOfLifeFade) && !isFading)
        {
            isFading = true;
            StartCoroutine("Fade", BulletLifeTime * (1 - PercentOfLifeFade));
        }

        curBulletLifeTime -= Time.deltaTime;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (curCollisions >= CollisionsLife)
        {
            //ObjectPooler.current.DestroyObject(gameObject);
        }
        else curCollisions++;
    }
}