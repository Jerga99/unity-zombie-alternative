using System.Collections.Generic;
using Eincode.ZombieSurvival.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace Eincode.ZombieSurvival.UI
{
    public class AbilityContainer : MonoBehaviour
    {
        public AbilityHolder[] abilities;

        [Header("Broadcasting")]
        [SerializeField]
        private IntEventChannelSO _upgradeSelectEvent;

        private void Start()
        {
            gameObject.SetActive(false);

            for (var i = 0; i < abilities.Length; i++)
            {
                var ability = abilities[i];
                ability.abilityIndex = i;
                ability.clickEvent = new UnityEvent<int>();
                ability.clickEvent.AddListener((abilityIndex) =>
                {
                    _upgradeSelectEvent.RaiseEvent(abilityIndex);
                });
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void DisplayNewAbility(int abilityIndex, AbilitySO newAbility)
        {
            var text = $"New Ability - {newAbility.Name}";
            abilities[abilityIndex].ChangeText(text);
        }

        public void DisplayUpgrade(int abilityIndex, AbilityUpgrade upgrade)
        {
            var measure = upgrade.Variant.Measure == UpgradeMeasure.Percentage ? "%" : "units";

            var text = $"Upgrades " +
                $"{upgrade.OriginSO.Name} " +
                $"{upgrade.Variant.UpgradeType} by " +
                $"{upgrade.Variant.Value} " +
                $"{measure}";

            abilities[abilityIndex].ChangeText(text);
        }
    }
}
