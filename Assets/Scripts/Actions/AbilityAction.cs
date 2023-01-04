using UnityEngine;
using Eincode.ZombieSurvival.Enemy;

namespace Eincode.ZombieSurvival.Actions
{
    public class AbilityAction : MonoBehaviour
    {
        internal bool isDelayed = false;
        internal bool canDealDamage = true;
        internal bool hasBeenReleasedToPool = false;
        internal LayerMask collideWith;
        internal Ability OriginAbility;

        internal float range;
        internal float distanceTraveled;
        internal string sourceTag;
        internal Vector3 direction;

        public virtual void Release()
        {
            if ((OriginAbility.OriginSO as PoolableAbilitySO).Pool == null)
            {
                Destroy(gameObject);
            }
            else if (!hasBeenReleasedToPool)
            {
                (OriginAbility.OriginSO as PoolableAbilitySO).Pool.Release(gameObject);
                hasBeenReleasedToPool = true;
            }

            canDealDamage = false;
            isDelayed = false;
        }

        public void OnAbilityActivation()
        {
            canDealDamage = true;
        }

        private void Update()
        {
            if (OriginAbility.OriginSO.ActionModifiers == null) { return; }

            foreach (var modifier in OriginAbility.OriginSO.ActionModifiers)
            {
                modifier.UpdateAction(this);
            }
        }

        private void OnEnable()
        {
            hasBeenReleasedToPool = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((collideWith.value & (1 << collision.transform.gameObject.layer)) > 0)
            {
                if (collision.TryGetComponent<Damageable>(out var damageable))
                {
                    // change enemy and player to Damageable!
                    if (isDelayed)
                    {
                        damageable.DelayAction(this);
                    }
                    else
                    {
                        damageable.TakeDamage(OriginAbility.Damage);
                        Release();
                    }
                };
            }
        }
    }
}