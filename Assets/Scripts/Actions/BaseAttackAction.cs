using UnityEngine;
using Eincode.ZombieSurvival.Enemy;
using Eincode.ZombieSurvival.Manager;

namespace Eincode.ZombieSurvival.Actions
{
    public class BaseAttackAction : AbilityAction
    {
        private SpriteRenderer _sprite;

        private void Start()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (collision.TryGetComponent<EnemyBehaviour>(out var enemy))
                {
                    enemy.TakeDamage(OriginAbility.Damage);
                }
            }
        }

        private void Update()
        {
            _sprite.flipX = SceneManager.Instance.Player.GetFlipX();
        }
    }
}

