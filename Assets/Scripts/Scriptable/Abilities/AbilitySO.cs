using System.Collections.Generic;
using Eincode.ZombieSurvival.Actions;
using Eincode.ZombieSurvival.Manager;
using UnityEngine;

public enum AbilityUpgradeType
{
    Damage,
    Cooldown,
    Units,
    Duration,
}

public enum UpgradeMeasure
{
    Percentage,
    Unit
}

[System.Serializable]
public struct AbilityUpgradeVariant
{
    public AbilityUpgradeType UpgradeType;
    public int Value;
    public UpgradeMeasure Measure;
}

public abstract class AbilitySO : BaseAbilitySO
{
    public GameObject Prefab;
    public ActionModifierSO[] ActionModifiers;
    public List<AbilityUpgradeVariant> PossibleUpgrades;

    [SerializeField] private IntValueSO _damage;
    [SerializeField] private IntValueSO _units;

    public int Units => _units.RuntimeValue;
    public int Damage => _damage.RuntimeValue;

    public GameObject Instantiate(Vector3 position, Quaternion rotation)
    {
        return Instantiate(Prefab, position, rotation);
    }

    public virtual Ability GetAbility(MonoBehaviour runner)
    {
        var ability = CreateAbility();
        ability.image = AbilityImage;

        ability._overallCooldown = Cooldown;

        ability._damage = _damage != null ? Damage : 0;
        ability._units = _units != null ? Units : 0;

        ability._originSO = this;

        ability.Awake(runner);
        return ability;
    }

    protected abstract Ability CreateAbility();
}

public abstract class AbilitySO<T> : AbilitySO where T : Ability, new()
{
    protected override Ability CreateAbility() => new T();
}

public abstract class Ability : BaseAbility
{
    public AbilitySO OriginSO => _originSO;
    internal AbilitySO _originSO;

    internal int _damage;
    internal int _units;

    public bool IsPassive => OriginSO.IsPassive;

    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public int Units
    {
        get { return _units; }
        set { _units = value; }
    }

    public void UpdateAbility(AbilityUpgrade upgrade)
    {
        if (upgrade.OriginSO.Name != OriginSO.Name)
        {
            return;
        }

        switch (upgrade.Variant.UpgradeType)
        {
            case AbilityUpgradeType.Damage:
                if (upgrade.Variant.Measure == UpgradeMeasure.Percentage)
                {
                    Damage += (Mathf.CeilToInt((upgrade.Variant.Value / 100f) * 10));
                }
                else
                {
                    Damage += upgrade.Variant.Value;
                }
                break;
            case AbilityUpgradeType.Cooldown:
                if (upgrade.Variant.Measure == UpgradeMeasure.Percentage)
                {
                    OverallCooldown -= (OverallCooldown * (upgrade.Variant.Value / 100f));
                }
                else
                {
                    OverallCooldown -= upgrade.Variant.Value;
                }
                break;
            case AbilityUpgradeType.Units:
                if (upgrade.Variant.Measure == UpgradeMeasure.Percentage)
                {
                    Units += (Mathf.CeilToInt((upgrade.Variant.Value / 100f) * 10));
                }
                else
                {
                    Units += upgrade.Variant.Value;
                }
                break;
        }
    }
}

public abstract class Ability<T> : Ability where T : AbilityAction
{
    internal MonoBehaviour _runner;
    internal T _action;

    internal GameObject InstantiateAction(out T action)
    {
        GameObject go;
        if (OriginSO is PoolableAbilitySO)
        {
            go = (OriginSO as PoolableAbilitySO).Pool.Get();
        }
        else
        {
            go = OriginSO.Instantiate(_runner.transform.position, Quaternion.identity);
        }

        if (!go.TryGetComponent(out action))
        {
            action = go.AddComponent<T>();
            action.sourceTag = _runner.tag;
            action.OriginAbility = this;
            action.collideWith = OriginSO.CollideWith;
            _action = action;
        }

        return go;
    }
}


