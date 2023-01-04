using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace Eincode.ZombieSurvival.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public enum SpawnSide
        {
            Top, Right, Bottom, Left
        }

        public bool collectionChecks = true;
        public int maxPoolSize = 50;

        // move to game manager ???
        public float SpawInterval = 2f;

        [SerializeField]
        private IntValueSO _gameStage;

        [SerializeField]
        private SpawnConfigurationSO _spawnConfiguration;

        [Range(0, 5)]
        public float SpawnRange;

        private float _spawnInterval;
        private readonly SpawnSide[] _side = { SpawnSide.Top, SpawnSide.Right, SpawnSide.Bottom, SpawnSide.Left };

        IObjectPool<GameObject> m_Pool;

        public IObjectPool<GameObject> Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    m_Pool = new ObjectPool<GameObject>(
                        CreatePooledItem,
                        OnTakeFromPool,
                        OnReturnedToPool,
                        OnDestroyPoolObject,
                        collectionChecks,
                        10,
                        maxPoolSize
                    );
                }

                return m_Pool;
            }
        }

        private void Start()
        {
            _spawnInterval = SpawInterval;

            //Invoke(nameof(ChangeState), 5.0f);
        }

        private void ChangeState()
        {
            _gameStage.RuntimeValue += 1;
        }

        void Update()
        {
            _spawnInterval -= Time.deltaTime;
            if (_spawnInterval < 0)
            {
                _spawnInterval = SpawInterval;
                Pool.Get();
            }
        }

        GameObject CreatePooledItem()
        {
            Vector2 spawnPosition = GetRandomPosition();

            var enemy = Instantiate(
                _spawnConfiguration.Waves[_gameStage.RuntimeValue - 1].Prefab,
                spawnPosition,
                Quaternion.identity
            );

            var damageable = enemy.GetComponent<Damageable>();
            damageable.pool = Pool;
            return enemy;
        }

        private Vector2 GetRandomPosition()
        {
            float spawnX;
            float spawnY;

            SpawnSide side = _side[Random.Range(0, _side.Length)];

            if (side == SpawnSide.Top)
            {
                spawnX = Random.Range(0 - SpawnRange, 1.0f + SpawnRange);
                spawnY = Random.Range(1.0f, 1.0f + SpawnRange);
            }
            else if (side == SpawnSide.Right)
            {
                spawnX = Random.Range(1.0f, 1.0f + SpawnRange);
                spawnY = Random.Range(0, 1.0f);
            }
            else if (side == SpawnSide.Bottom)
            {
                spawnX = Random.Range(0 - SpawnRange, 1.0f + SpawnRange);
                spawnY = Random.Range(0f, 0 - SpawnRange);
            }
            else
            {
                spawnX = Random.Range(0 - SpawnRange, 0);
                spawnY = Random.Range(0, 1.0f);
            }

            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(spawnX, spawnY, -10.0f));

            return v3Pos;
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(GameObject go)
        {
            go.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(GameObject go)
        {
            // TODO: REFACTOR, should itwather with Damageable?
            if (go.TryGetComponent<EnemyBehaviour>(out var behaviour))
            {
                if (behaviour.gameStageToSpawn == _gameStage.RuntimeValue)
                {
                    go.SetActive(true);
                    behaviour.Activate();
                    go.transform.position = GetRandomPosition();
                }
                else
                {
                    Destroy(go);
                }
            };
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(GameObject go)
        {
            Destroy(go);
        }

        public float explosionRadius = 0.5f;
        public float gizmoX = 1.0f;
        public float gizmoY = 1.0f;
        public float gizmoZ = 10.0f;

        void OnDrawGizmosSelected()
        {
            Camera camera = Camera.main;
            // Display the explosion radius when selected
            Gizmos.color = new Color(1, 1, 0, 0.75F);
            Gizmos.DrawSphere(camera.ViewportToWorldPoint(new Vector3(gizmoX, gizmoY, gizmoZ)), explosionRadius);
        }
    }
}

