using UnityEngine;
using Eincode.ZombieSurvival.Actions;
using Eincode.ZombieSurvival.UI;
using Eincode.Arena.Abilities;
using System.Collections.Generic;

namespace Eincode.ZombieSurvival.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public int Health { get { return _healthSO.CurrentHealth; } }
        public int MaxHealth { get { return _healthSO.MaxHealth; } }

        public List<Ability> Abilities => _abilityRunner.Abilities;

        public float CoinDetectionRange;
        public ExperienceBar ExpBar;
        public int experienceToLevel;

        [Header("Channeling")]
        [SerializeField]
        private IntEventChannelSO _levelUpEvent;

        [Header("Health")]
        [SerializeField] private IntValueSO _initialHealth;
        [SerializeField] private HealthSO _healthSO;

        [Header("Level")]
        [SerializeField] private IntValueSO _levelSO;
        [SerializeField] private IntValueSO _currentExprienceSO;

        private SpriteRenderer _sprite;
        private AbilityRunner _abilityRunner;
        private Damageable _damageable;

        private void Awake()
        {
            _healthSO.SetMaxHealth(_initialHealth.InitialValue);
            _healthSO.SetCurrentHealth(_initialHealth.InitialValue);
        }

        private void Start()
        {
            _damageable = GetComponent<Damageable>();
            _sprite = GetComponent<SpriteRenderer>();
            _abilityRunner = GetComponent<AbilityRunner>();
        }

        private void Update()
        {
            MarkNearbyItemsAsSelected();
        }

        private void MarkNearbyItemsAsSelected()
        {
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, CoinDetectionRange);
            foreach (Collider2D collider in colliderArray)
            {
                if (collider.TryGetComponent(out DropItem item)
                    && !item.isTargeted && !item.isStatic)
                {
                    item.isTargeted = true;
                }
            }
        }

        public void SetExprience(int exprience)
        {
            _currentExprienceSO.RuntimeValue += exprience;

            if (_currentExprienceSO.RuntimeValue == experienceToLevel)
            {
                experienceToLevel = _currentExprienceSO.RuntimeValue + (_levelSO.RuntimeValue * 10);
                _levelSO.RuntimeValue += 1;
                _currentExprienceSO.RuntimeValue = 0;
            }

            ExpBar.SetValue(_currentExprienceSO.RuntimeValue, experienceToLevel);
        }

        public Ability AddAbility(AbilitySO ability)
        {
            return _abilityRunner.AddAbility(ability);
        }

        public bool GetFlipX()
        {
            return _sprite.flipX;
        }

        public void TakeDamage(int damage)
        {
            _damageable.TakeDamage(damage);
        }
    }
}