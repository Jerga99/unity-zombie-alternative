using System;
using System.Collections.Generic;
using Eincode.ZombieSurvival.Manager;
using UnityEngine;

// OBSOLETE
public class AbilityTracker
{
    public struct AbilityStats
    {
        public float Cooldown;
        public int Units;
        public int Damage;
        public int NumOfUpgrades;

        public AbilityStats(float cooldown, int units, int damage, int numOfUpgrades)
        {
            Cooldown = cooldown;
            Units = units;
            Damage = damage;
            NumOfUpgrades = numOfUpgrades;
        }
    }

    public Dictionary<string, AbilityStats> AbilityNameToStats => _abilityNameToStats;
    Dictionary<string, AbilityStats> _abilityNameToStats = new();

    public void Register(Ability ability)
    {
        var abilityStats = new AbilityStats(
            ability.OverallCooldown,
            ability.Units,
            ability.Damage,
            0
            );

        _abilityNameToStats[ability.OriginSO.Name] = abilityStats;
    }

    public void UpdateAbility(AbilityUpgrade upgrade)
    {
        var abilityStats = _abilityNameToStats[upgrade.OriginSO.Name];

        switch (upgrade.Variant.UpgradeType)
        {
            case AbilityUpgradeType.Damage:
                if (upgrade.Variant.Measure == UpgradeMeasure.Percentage)
                {
                    abilityStats.Damage += (Mathf.CeilToInt((upgrade.Variant.Value / 100f) * 10));
                }
                else
                {
                    abilityStats.Damage += upgrade.Variant.Value;
                }
                break;
            case AbilityUpgradeType.Cooldown:
                if (upgrade.Variant.Measure == UpgradeMeasure.Percentage)
                {
                    abilityStats.Cooldown -= (abilityStats.Cooldown * (upgrade.Variant.Value / 100f));
                }
                else
                {
                    abilityStats.Cooldown -= upgrade.Variant.Value;
                }



                break;
            case AbilityUpgradeType.Units:
                if (upgrade.Variant.Measure == UpgradeMeasure.Percentage)
                {
                    abilityStats.Units += (Mathf.CeilToInt((upgrade.Variant.Value / 100f) * 10));
                }
                else
                {
                    abilityStats.Units += upgrade.Variant.Value;
                }
                break;
        }

        abilityStats.NumOfUpgrades++;
        _abilityNameToStats[upgrade.OriginSO.Name] = abilityStats;
    }
}

