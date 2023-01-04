using UnityEngine;
using UnityEngine.UI;
using Eincode.ZombieSurvival.Manager;

namespace Eincode.ZombieSurvival.UI
{
    public class HealthBarBehaviour : SliderBarBehaviour
    {
        public HealthSO _playerHealth;

        public override void SetValue(float health, float maxHealth)
        {
            base.SetValue(health, maxHealth);
            Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green, Slider.normalizedValue);
        }

        private void Update()
        {
            SetValue(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
            transform.position = SceneManager.Instance.Player.transform.position + Offset;
        }
    }
}

