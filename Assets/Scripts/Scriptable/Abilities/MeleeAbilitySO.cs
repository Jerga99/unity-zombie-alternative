using Eincode.ZombieSurvival.Actions;
using UnityEngine;

namespace Eincode.ZombieSurvival.Scriptable
{
    [CreateAssetMenu(
        fileName = "MeleeAttack",
        menuName = "Abilities/MeleeAttack"
    )]
    public class MeleeAbilitySO : AbilitySO<MeleeAbility>
    {
        protected new virtual Ability CreateAbility()
        {
            return new MeleeAbility();
        }
    }

    public class MeleeAbility : Ability<MeleeAttackAction>
    {
        public override void Awake(MonoBehaviour runner)
        {
            _runner = runner;
            TriggerAbility();
        }

        protected override void TriggerAbility()
        {
            var ability = InstantiateAction(out var _);
            ability.transform.parent = _runner.transform;
        }

        public override void Start()
        {
            base.Start();
            _action.gameObject.SetActive(true);
        }
    }
}

