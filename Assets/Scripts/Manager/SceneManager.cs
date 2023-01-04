using UnityEngine;
using Eincode.ZombieSurvival.Player;
using System.Collections.Generic;
using Eincode.ZombieSurvival.UI;
using Eincode.ZombieSurvival.Utils;

// TODO: Change the way how updates are applied, centralized update monitor!

namespace Eincode.ZombieSurvival.Manager
{
    public class UpgradeMap : Dictionary<
        AbilityUpgradeType?,
        Dictionary<string, List<int>>
    >
    { }

    public class PossibleUpgrade
    {
        public AbilitySO OriginSO;
        public bool isNewAbility = false;

        public PossibleUpgrade(AbilitySO abilitySO)
        {
            OriginSO = abilitySO;
        }
    }

    public class AbilityUpgrade : PossibleUpgrade
    {
        public AbilityUpgradeVariant Variant;

        public AbilityUpgrade(AbilityUpgradeVariant variant, AbilitySO originSO) : base(originSO)
        {
            Variant = variant;
        }
    }

    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; private set; }
        public PlayerBehaviour Player => _player;

        public AbilityContainer AbilityContainer;
        public List<AbilitySO> AbilityPool;

        // TODO: REWORK
        public UpgradeMap AbilityUpgrades = new();

        [SerializeField]
        private IntValueSO _gameStage;

        [Header("Listening")]
        [SerializeField]
        private IntEventChannelSO _upgradeSelectEvent;

        [SerializeField]
        private IntEventChannelSO _levelUpEvent;

        [Header("Channeling")]
        [SerializeField]
        private AbilityUpgradeEventChannelSO _abilityUpgradeEvent;

        private PlayerBehaviour _player;

        private PossibleUpgrade[] _upgrades;

        // Use this for initialization
        void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }
            _player = FindObjectOfType<PlayerBehaviour>();

            _upgradeSelectEvent.OnEventRaised += OnAbilitySelection;

            foreach (var ut in EincodeUtils.GetValues<AbilityUpgradeType>())
            {
                AbilityUpgrades[ut] = new Dictionary<string, List<int>>();
            }
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
        }

        public void OnAbilitySelection(int abilityIndex)
        {
            PossibleUpgrade possibleUpgrade = _upgrades[abilityIndex];

            if (possibleUpgrade.isNewAbility)
            {
                Player.AddAbility(possibleUpgrade.OriginSO);
            }
            else
            {
                var upgrade = possibleUpgrade as AbilityUpgrade;
                var upgradeType = upgrade.Variant.UpgradeType;

                if (!AbilityUpgrades[upgradeType].ContainsKey(upgrade.OriginSO.Name))
                {
                    AbilityUpgrades[upgradeType].Add(upgrade.OriginSO.Name, new List<int>());
                }

                AbilityUpgrades[upgradeType][upgrade.OriginSO.Name].Add(upgrade.Variant.Value);

                _abilityUpgradeEvent.RaiseEvent(upgrade);
            }

            ResumeGame();
            AbilityContainer.Hide();
        }

        public PossibleUpgrade[] GiveRewardToPlayer()
        {
            var abilityIdx = 0;
            var playerAbilitiesSO = Player.Abilities
                .ConvertAll((a => a.OriginSO))
                .FindAll(a => a.PossibleUpgrades.Count > 0);

            var playerAbilitiesIdx = playerAbilitiesSO.ConvertAll((_ => abilityIdx++));
            var availableAbilities = AbilityPool.FindAll((ability) => !playerAbilitiesSO.Contains(ability));

            List<AbilitySO> newAbilities = new();

            if (availableAbilities.Count >= 2)
            {
                int numOfNewAbilites = Random.Range(1, 2);
                for (var i = 0; i < numOfNewAbilites; i++)
                {
                    var abilityId = Random.Range(0, availableAbilities.Count);
                    newAbilities.Add(availableAbilities[abilityId]);
                    availableAbilities.RemoveAt(abilityId);
                }
            }
            else if (availableAbilities.Count == 1)
            {
                newAbilities.Add(availableAbilities[0]);
                availableAbilities.RemoveAt(0);
            }

            int numOfUpgrades = 3 - newAbilities.Count;
            var upgrades = new PossibleUpgrade[3];

            AbilityContainer.Show();

            for (int i = 0; i < numOfUpgrades; i++)
            {
                int rndIdx = Random.Range(0, playerAbilitiesIdx.Count);
                int abilityUpgradeIdx = playerAbilitiesIdx[rndIdx];
                int rndUpgradeTypeIndex = Random.Range(0, playerAbilitiesSO[abilityUpgradeIdx].PossibleUpgrades.Count);

                var upgrade = new AbilityUpgrade(
                    playerAbilitiesSO[abilityUpgradeIdx].PossibleUpgrades[rndUpgradeTypeIndex],
                    playerAbilitiesSO[abilityUpgradeIdx]
                );

                upgrades[i] = upgrade;
                playerAbilitiesIdx.RemoveAt(rndIdx);
            }

            if (newAbilities.Count > 0)
            {
                for (var i = 0; i < newAbilities.Count; i++)
                {
                    var upgrade = new PossibleUpgrade(
                        newAbilities[i]
                    )
                    {
                        isNewAbility = true
                    };

                    upgrades[numOfUpgrades + i] = upgrade;
                }
            }

            _upgrades = EincodeUtils.Shuffle(upgrades);

            for (var i = 0; i < upgrades.Length; i++)
            {
                var upgrade = upgrades[i];

                if (upgrade.isNewAbility)
                {
                    AbilityContainer.DisplayNewAbility(i, upgrade.OriginSO);
                }
                else
                {
                    AbilityContainer.DisplayUpgrade(i, upgrade as AbilityUpgrade);
                }
            }

            return _upgrades;
        }
    }
}
