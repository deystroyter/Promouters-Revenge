using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private ObjectPooler.ObjectInfo.ObjectType enemyType;

        [SerializeField] private float _minTimeBetweenSpawn = 0.5f;
        [SerializeField] private float _maxTimeBetweenSpawn = 2f;

        private GameObject[] _spawnPoints;

        private IEnumerator RandomPeriodSpawn()
        {
            do
            {
                Spawn(GetRandomNavMeshSpawnPoint());
                yield return new WaitForSeconds(Random.Range(_minTimeBetweenSpawn, _maxTimeBetweenSpawn));
            } while (true);
        }

        protected void Start()
        {
            _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            StartEnemySpawner();
        }

        // Update is called once per frame
        protected void Update()
        {
        }

        private void StartEnemySpawner()
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

        private Vector3 GetRandomNavMeshSpawnPoint()
        {
            var rndSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            if (NavMesh.SamplePosition(rndSpawnPoint.transform.position, out var hit, 10.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
            else
            {
                throw new System.Exception($"Something wrong with SpawnPoint -- {rndSpawnPoint.name}");
            }
        }

        public void StopEnemySpawner()
        {
            StopCoroutine("RandomPeriodSpawner");
        }
    }
}