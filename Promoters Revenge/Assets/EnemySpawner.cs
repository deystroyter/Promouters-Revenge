using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    public ObjectPooler.ObjectInfo.ObjectType EnemyType;

    [SerializeField]
    public float MaxTimeBetweenSpawn = 2f;

    public Transform SpawnTransform;

    private IEnumerator RandomPeriodSpawner()
    {
        Spawn();
        yield return new WaitForSeconds(Random.Range(0.5f, MaxTimeBetweenSpawn));
    }

    // Update is called once per frame
    private void Update()
    {
        StartCoroutine("RandomPeriodSpawner");
    }

    public void Spawn()
    {
        GameObject Enemy = ObjectPooler.current.GetObject(EnemyType);
        if (Enemy == null) return;
        Enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 v = Vector3.zero;
        v.x = Random.Range(0f, 100.0f);
        v.z = Random.Range(0f, 100.0f);
        Enemy.transform.position = v;
        Enemy.SetActive(true);
    }

    public void StopEnemySpawner()
    {
        StopCoroutine("RandomPeriodSpawner");
    }
}