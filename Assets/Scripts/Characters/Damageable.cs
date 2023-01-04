using UnityEngine;
using System.Collections;
using Eincode.ZombieSurvival.Manager;
using Eincode.ZombieSurvival.Sprite;
using System.Collections.Generic;
using UnityEngine.Pool;
using Eincode.ZombieSurvival.Actions;

public class Damageable : MonoBehaviour
{
    public bool IsDeath { get { return _healthSO.CurrentHealth <= 0; } }

    public int Health { get { return _healthSO.CurrentHealth; } }
    public int MaxHealth { get { return _healthSO.MaxHealth; } }

    public bool ShowDamage;
    public Transform HitEffect;

    public Color FlashColor;

    private SpriteFlash _flashEffect;
    private Lootable _lootable;
    private Animator _animator;
    private int _animIDDeath;

    [Header("Health")]
    [SerializeField] private IntValueSO _initialHealth;
    // set only in case of player
    [SerializeField] private HealthSO _healthSO;

    private List<AbilityAction> _delayedActions;

    [HideInInspector]
    public IObjectPool<GameObject> pool;

    private void Awake()
    {
        if (_healthSO == null)
        {
            _healthSO = ScriptableObject.CreateInstance<HealthSO>();
        }

        _healthSO.SetMaxHealth(_initialHealth.InitialValue);
        _healthSO.SetCurrentHealth(_initialHealth.InitialValue);
    }

    // Use this for initialization
    void Start()
    {
        if (HitEffect != null)
        {
            HitEffect.gameObject.SetActive(false);
        }

        TryGetComponent(out _lootable);
        _delayedActions = new List<AbilityAction>();
        _animIDDeath = Animator.StringToHash("Death");
        _flashEffect = GetComponent<SpriteFlash>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        for (int i = _delayedActions.Count - 1; i >= 0; i--)
        {
            var action = _delayedActions[i];

            if (IsDeath)
            {
                _delayedActions.Remove(action);
                break;
            }

            if (action.canDealDamage)
            {
                TakeDamage(action.OriginAbility.Damage);
                _delayedActions.Remove(action);
            }
        }
    }

    public void Release()
    {
        if (pool == null)
        {
            Destroy(gameObject);
        }
        else
        {
            pool.Release(gameObject);
        }

        _delayedActions.Clear();
    }

    public void DelayAction(AbilityAction action)
    {
        _delayedActions.Add(action);
    }

    public void Activate()
    {
        _healthSO.SetCurrentHealth(_initialHealth.InitialValue);
    }

    public void TakeDamage(int damage)
    {
        if (IsDeath)
        {
            return;
        }

        if (_healthSO.CurrentHealth - damage <= 0)
        {
            _animator.SetTrigger(_animIDDeath);

            if (_lootable != null)
            {
                _lootable.DropItem();
            }
        }

        _healthSO.InflictDamage(damage);

        if (_flashEffect != null)
        {
            _flashEffect.Flash(FlashColor);
        }

        if (HitEffect != null)
        {
            HitEffect.gameObject.SetActive(true);
        }

        if (ShowDamage)
        {
            UIManager.Instance.ShowDamage(damage, transform);
        }
    }
}

