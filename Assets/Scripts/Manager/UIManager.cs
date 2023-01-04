using UnityEngine;
using TMPro;
using Eincode.ZombieSurvival.UI;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Eincode.ZombieSurvival.Manager
{

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public GameObject DamageTextPrefab;

        public RectTransform PassiveAbilityContainer;
        public RectTransform ActiveAbilityContainer;
        public RectTransform Canvas;

        public string NiceTime
        {
            get
            {
                int minutes = Mathf.FloorToInt(_gameTime.RuntimeValue / 60F);
                int seconds = Mathf.FloorToInt(_gameTime.RuntimeValue - minutes * 60);
                string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
                return niceTime;
            }
        }

        [SerializeField]
        private FloatValueSO _gameTime;

        [SerializeField]
        private IntValueSO _level;

        [Header("Listening")]
        [SerializeField]
        private AbilityEventChannelSO _abilityAddEvent;

        public TextMeshProUGUI TimerText;
        public TextMeshProUGUI LevelText;

        private List<Image> _abilityIcons = new();
        private Dictionary<string, int> _iconOffsets = new();

        // Use this for initialization
        void Awake()
        {
            _iconOffsets.Add(PassiveAbilityContainer.name, 0);
            _iconOffsets.Add(ActiveAbilityContainer.name, 0);

            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }

            _abilityAddEvent.OnEventRaised += OnAbilityAdded;
        }

        private void OnAbilityAdded(Ability ability)
        {
            var Container = ability.IsPassive ? PassiveAbilityContainer : ActiveAbilityContainer;

            Image icon = Instantiate(ability.image, Container.transform);
            AbilityIcon aIcon = icon.GetComponent<AbilityIcon>();

            aIcon.InitIcon(
                icon,
                () => ability.OverallCooldown,
                () => ability.CurentCooldown
            );

            icon.rectTransform.anchoredPosition = new Vector2(
                icon.rectTransform.anchoredPosition.x,
                _iconOffsets[Container.name]
            );

            _iconOffsets[Container.name] -= ((int)aIcon.ImageSize + 10);

            icon.transform.SetParent(Container.transform, false);
            _abilityIcons.Add(icon);
        }

        void Update()
        {
            _gameTime.RuntimeValue += Time.deltaTime;
            TimerText.text = NiceTime;
            LevelText.text = $"lvl: {_level.RuntimeValue}";
        }

        public void ShowDamage(int damage, Transform target)
        {
            var damageText = Instantiate(DamageTextPrefab, Canvas.transform)
                .GetComponent<DamageDisplay>();

            damageText.ShowDamage(damage, target, Canvas);
        }
    }
}
