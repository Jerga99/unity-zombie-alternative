using UnityEngine;

namespace Eincode.ZombieSurvival.Scriptable
{
    [CreateAssetMenu(
        fileName = "AnimationAbility",
        menuName = "Abilities/Animation Ability"
    )]
    // TODO: Ability should also sopecify how AnimationAbility speed will increase during the roll??
    public class AnimationAbilitySO : AbilitySO<AnimationAbility>
    {
        public string AnimatorParameter;

        protected override Ability CreateAbility()
        {
            return new AnimationAbility(AnimatorParameter);
        }
    }

    public class AnimationAbility : Ability
    {
        private readonly int _animIDRoll;
        private Animator _animator;

        public AnimationAbility() { }
        public AnimationAbility(string animatorParam)
        {
            _animIDRoll = Animator.StringToHash(animatorParam);
        }

        public override void Awake(MonoBehaviour runner)
        {
            _animator = runner.GetComponent<Animator>();
        }

        protected override void TriggerAbility()
        {
            _animator.SetTrigger(_animIDRoll);
        }

        public override void Start()
        {
            base.Start();
            TriggerAbility();
        }
    }
}

