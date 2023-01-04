using System.Collections;
using Eincode.ZombieSurvival.Actions;
using UnityEngine;
using UnityEngine.Pool;

namespace Eincode.ZombieSurvival.Scriptable
{
    [CreateAssetMenu(
        fileName = "Data",
        menuName = "ScriptableObjects/ExperienceReward",
        order = 3)
    ]
    public class ExperienceReward : Reward
    {
        public bool collectionChecks = true;
        public int maxPoolSize = 50;
        public int experience;

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
                        10
                    );
                }

                return m_Pool;
            }
        }

        public override void Drop(MonoBehaviour source)
        {
            var actionGo = Pool.Get();
            var dropItem = actionGo.GetComponent<DropItem>();

            dropItem.exprience = experience;
            actionGo.transform.position = source.transform.position;
        }


        GameObject CreatePooledItem()
        {
            var actionGo = Instantiate(RewardPrefab);
            var dropItem = actionGo.GetComponent<DropItem>();

            dropItem.pool = Pool;

            return actionGo;
        }

        void OnReturnedToPool(GameObject go)
        {
            go.SetActive(false);
        }

        void OnTakeFromPool(GameObject go)
        {
            go.SetActive(true);
        }

        void OnDestroyPoolObject(GameObject go)
        {
            Destroy(go);
        }
    }
}