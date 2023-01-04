using UnityEngine;
using Eincode.ZombieSurvival.Utils;
using Eincode.Arena.Abilities;


// TODO: Spawn Manager + Abilities to Enemies, Multiple Ways, Increase Spawn Timers

// TODO: Recheck spawner of enemies and check spawned enemies waves, bosses, special abilities

// menu
// sounds
// keep track of death enemies and damage dealt /// statistics!
// map design

// TODO: Check all of your code and look for improvements, abilities

namespace Eincode.ZombieSurvival.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float TargetSpeed = 1.0f;

        public Vector2 moveVector;
        public bool isRolling;

        private Animator _animator;

        // animation IDs
        private int _animIDSpeed;

        private bool _facingRight;
        private float _speedBlend;
        private SpriteRenderer _sprite;
        private AbilityRunner _abilityRunner;


        [SerializeField]
        private InputReader _inputReader;

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
        }

        private void OnEnable()
        {
            _inputReader.RollEvent += OnRolleEvent;
            _inputReader.MoveEvent += OnMoveEvent;
        }

        private void OnDisable()
        {
            _inputReader.RollEvent -= OnRolleEvent;
            _inputReader.MoveEvent -= OnMoveEvent;
        }

        // Use this for initialization
        void Start()
        {
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
            _abilityRunner = GetComponent<AbilityRunner>();

            AssignAnimationIDs();
        }

        void Update()
        {
            Move();
        }

        private void OnRolleEvent(bool isRoll)
        {
            if (isRoll)
            {
                _abilityRunner.RunRollAbility();
            }
        }

        private void OnMoveEvent(Vector2 move)
        {
            moveVector = move;
        }

        private void Move()
        {
            float targetSpeed = isRolling ? TargetSpeed * 2 : TargetSpeed;

            if (moveVector == Vector2.zero)
            {
                targetSpeed = 0;
            }

            _speedBlend = Mathf.Lerp(_speedBlend, targetSpeed, Time.deltaTime * 10.0f);

            var move = new Vector3(moveVector.x, moveVector.y, 0).normalized;
            var velocity = move * Time.deltaTime * targetSpeed;

            transform.position += velocity;

            if (moveVector.x < 0 && !_facingRight)
            {
                Flip();
            }
            else if (moveVector.x > 0 && _facingRight)
            {
                Flip();
            }

            _animator.SetFloat(_animIDSpeed, _speedBlend);
        }

        private void Flip()
        {
            EincodeUtils.Flip(_sprite, () => _facingRight = !_facingRight);
        }
    }

}
