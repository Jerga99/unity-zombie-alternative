using Eincode.ZombieSurvival.Actions;
using Eincode.ZombieSurvival.Manager;
using Eincode.ZombieSurvival.Utils;
using UnityEngine;

namespace Eincode.ZombieSurvival.Scriptable
{
    [CreateAssetMenu(
        fileName = "SpawnInRadiusAbility",
        menuName = "Abilities/Spawn In Radius Ability"
    )]
    public class SpawnInRadiusAbilitySO : PoolableAbilitySO<SpawnInRadiusAbility>
    {
        public float MinRange;
        public float MaxRange;

        protected new virtual Ability CreateAbility() => new SpawnInRadiusAbility();
    }

    public class SpawnInRadiusAbility : Ability<AbilityAction>
    {
        new public SpawnInRadiusAbilitySO OriginSO => (SpawnInRadiusAbilitySO)base.OriginSO;

        public override void Awake(MonoBehaviour runner)
        {
            _runner = runner;
        }

        protected override void TriggerAbility()
        {
            var ability = InstantiateAction(out var action);

            if (OriginSO.IsDelayed)
            {
                action.canDealDamage = false;
                action.isDelayed = true;
            }

            Vector2 atPosition = EincodeUtils.RandomPointInAnnulus(
                _runner.transform.position,
                OriginSO.MinRange,
                OriginSO.MaxRange
            );

            ability.transform.position = atPosition;
        }

        public override void Start()
        {
            base.Start();

            for (var i = 0; i < Units; i++)
            {
                TriggerAbility();
            }
        }
    }
}

