using UnityEngine;
using UnityEngine.Pool;

// TODO: Re-think Poolable abilities
public abstract class PoolableAbilitySO : AbilitySO
{
    public bool collectionChecks = true;
    public int maxPoolSize = 50;

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

    GameObject CreatePooledItem()
    {
        var actionGo = Instantiate(Prefab);
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

public abstract class PoolableAbilitySO<T> : PoolableAbilitySO where T : Ability, new()
{
    protected override Ability CreateAbility() => new T();
}

