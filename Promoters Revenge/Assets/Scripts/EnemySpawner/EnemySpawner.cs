using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private ObjectPooler.ObjectInfo.ObjectType enemyType;

        [SerializeField] private float _maxTimeBetweenSpawn = 2f;

        private IEnumerator RandomPeriodSpawn()
        {
            Spawn(GetRandomPlace());
            yield return new WaitForSeconds(Random.Range(0.5f, _maxTimeBetweenSpawn));
        }

        // Update is called once per frame
        protected void Update()
        {
            SpawnLogic();
        }

        private void SpawnLogic()
        {
            StartCoroutine("RandomPeriodSpawn");
        }

        private void Spawn(Vector3 position)
        {
            if (ObjectPooler.ObjPooler.GetCountOfQueueObjects(enemyType) > 0)
            {
                GameObject Enemy = ObjectPooler.ObjPooler.GetObject(enemyType);

                if (Enemy == null)
                {
                    return;
                }

                Enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Enemy.transform.position = position;
                Enemy.SetActive(true);
            }
        }

        private Vector3 GetRandomPlace()
        {
            Vector3 v3 = Vector3.zero;
            v3.x = Random.Range(0f, 100.0f);
            v3.z = Random.Range(0f, 100.0f);
            return v3;
        }

        public void StopEnemySpawner()
        {
            StopCoroutine("RandomPeriodSpawner");
        }
    }
}