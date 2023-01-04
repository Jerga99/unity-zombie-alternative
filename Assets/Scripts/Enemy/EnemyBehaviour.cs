using UnityEngine;
using Eincode.ZombieSurvival.Manager;
using Eincode.ZombieSurvival.Utils;
using static Eincode.ZombieSurvival.Scriptable.Reward;

namespace Eincode.ZombieSurvival.Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public float SpeedModifier;
        public float ReactionTime;
        public bool IsDeath => _damageable.IsDeath;

        // TODO: OR NO SERIALIZABLE?
        [HideInInspector]
        public float AccuReactionTime;
        [HideInInspector]
        public bool FacingRight;

        // game stage
        public int gameStageToSpawn;

        [Header("Combat")]
        [SerializeField] private IntValueSO _damageSO;
        [SerializeField] private FloatValueSO _attackSpeedSO;

        private Vector3 _playerPosition;
        private SceneManager _sceneManager;
        private SpriteRenderer _sprite;
        private Damageable _damageable;
        private float _randomSpeedModifier;

        // TODO: REFACTOR
        public void Activate()
        {
            _damageable.Activate();
        }

        private void Awake()
        {
            _damageable = GetComponent<Damageable>();
        }

        private void Start()
        {
            AccuReactionTime = ReactionTime;
            _sprite = GetComponent<SpriteRenderer>();
            _sceneManager = SceneManager.Instance;
            _randomSpeedModifier = Random.Range(1.0f, 1.5f);
            //_delayedActions = new List<AbilityAction>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                InvokeRepeating(
                    nameof(DealDamage),
                    0.1f,
                    _attackSpeedSO.RuntimeValue
                );
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                CancelInvoke(nameof(DealDamage));
            }
        }

        private void DropItem(Loot loot)
        {
            var roll = Random.Range(0, 100);

            if (roll <= loot.dropChance)
            {
                loot.reward.Drop(this);
            }
        }

        private void DealDamage()
        {
            if (!_damageable.IsDeath)
            {
                _sceneManager.Player.TakeDamage(_damageSO.RuntimeValue);
            }
        }

        public void TakeDamage(int damage)
        {
            _damageable.TakeDamage(damage);
        }

        public void Flip()
        {
            EincodeUtils.Flip(_sprite, () => FacingRight = !FacingRight);
        }
    }
}