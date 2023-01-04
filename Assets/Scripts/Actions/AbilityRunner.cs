using System.Collections.Generic;
using UnityEngine;

namespace Eincode.Arena.Abilities
{
    public class AbilityRunner : MonoBehaviour
    {
        public List<Ability> Abilities => _abilities;

        [SerializeField]
        private AbilitySO[] _abilitiesSO;
        private List<Ability> _abilities;

        [Header("Listening")]
        [SerializeField]
        private AbilityUpgradeEventChannelSO _abilityUpgradeEvent;

        [Header("Channeling")]
        [SerializeField]
        private AbilityEventChannelSO _abilityAddEvent;

        private void Awake()
        {
            _abilities = new List<Ability>();

            foreach (var abilitySO in _abilitiesSO)
            {
                PrepareAbility(abilitySO);
            }
        }

        private void PrepareAbility(AbilitySO abilitySO)
        {
            var ability = abilitySO.GetAbility(this);

            if (_abilityAddEvent != null)
            {
                _abilityAddEvent.RaiseEvent(ability);
            }
            _abilities.Add(ability);

            if (_abilityAddEvent != null)
            {
                _abilityUpgradeEvent.OnEventRaised += ability.UpdateAbility;
            }
        }

        public Ability AddAbility(AbilitySO newAbilitySO)
        {
            var ability = newAbilitySO.GetAbility(this);

            _abilities.Add(ability);

            if (_abilityAddEvent != null)
            {
                _abilityAddEvent.RaiseEvent(ability);
            }

            if (_abilityUpgradeEvent != null)
            {
                _abilityUpgradeEvent.OnEventRaised += ability.UpdateAbility;
            }

            return ability;
        }

        public Ability FindAbility(string name)
        {
            return _abilities.Find((ability) => ability.OriginSO.Name == name);
        }

        public void RunRollAbility()
        {
            TriggerAbility("Roll");
        }

        // TODO: Return bool? Can ability trigger fail?
        private void TriggerAbility(string name)
        {
            var ability = FindAbility(name);

            if (ability == null)
            {
                return;
            }

            if (!ability.IsCooldownPending)
            {
                ability.Start();
            }
        }

        private void Update()
        {
            foreach (var ability in _abilities)
            {
                if (ability.IsCooldownPending)
                {
                    ability.Update();
                }
                else if (ability.IsPassive)
                {
                    ability.Start();
                }
            }
        }

        //void OnGUI()
        //{
        //    GUILayout.Label("Ability Tracker");

        //    foreach (var ability in _abilities)
        //    {
        //        var abilityName = ability.OriginSO.Name;

        //        GUILayout.Label(
        //            $"{abilityName}, " +
        //            $"D: {ability.Damage}, " +
        //            $"C: {ability.OverallCooldown}, " +
        //            $"CC: {ability.CurentCooldown}, " +
        //            $"U: {ability.Units}");
        //    }
        //}
    }
}
