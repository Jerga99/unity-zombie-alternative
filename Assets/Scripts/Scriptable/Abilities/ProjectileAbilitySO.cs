using Eincode.ZombieSurvival.Actions;
using UnityEngine;

namespace Eincode.ZombieSurvival.Scriptable
{
    [CreateAssetMenu(
        fileName = "ProjectileAbility",
        menuName = "Abilities/ProjectileAbility"
    )]
    public class ProjectileAbilitySO : PoolableAbilitySO<ProjectileAbility>
    {
        public float Range;
        protected new virtual Ability CreateAbility() => new ProjectileAbility();
    }

    public class ProjectileAbility : Ability<AbilityAction>
    {
        new public ProjectileAbilitySO OriginSO => (ProjectileAbilitySO)base.OriginSO;

        public override void Awake(MonoBehaviour runner)
        {
            _runner = runner;
        }

        protected override void TriggerAbility()
        {
            var ability = InstantiateAction(out var action);
            action.range = OriginSO.Range;
            ability.transform.position = _runner.transform.position; ;
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

