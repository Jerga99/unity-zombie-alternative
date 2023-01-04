using UnityEngine;
using UnityEngine.Pool;
using Eincode.ZombieSurvival.Manager;
using static Eincode.ZombieSurvival.Scriptable.Reward;

namespace Eincode.ZombieSurvival.Actions
{
    public class DropItem : MonoBehaviour
    {
        public IObjectPool<GameObject> pool;
        public RewardType rewardType;
        public bool isTargeted = false;
        public bool isStatic;
        public int exprience;

        private SceneManager _sceneManager;

        private void Awake()
        {
            _sceneManager = SceneManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            if (isTargeted && !isStatic)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    _sceneManager.Player.transform.position,
                    Time.deltaTime * 2.3f
                );
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (rewardType == RewardType.Experience)
                {
                    _sceneManager.Player.SetExprience(exprience);
                    pool.Release(gameObject);
                }
                else if (rewardType == RewardType.NewAbility)
                {
                    _sceneManager.PauseGame();
                    _sceneManager.GiveRewardToPlayer();
                }

                if (pool == null)
                {
                    Destroy(gameObject);
                }

                isTargeted = false;
            }
        }
    }
}
