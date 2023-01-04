using UnityEngine;
using TMPro;

namespace Eincode.ZombieSurvival.UI
{
    public class DamageDisplay : MonoBehaviour
    {
        private TextMeshProUGUI _damageText;
        private Transform _target;
        private RectTransform _canvasRect;

        private void Awake()
        {
            _damageText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void ShowDamage(
            int damage,
            Transform transform,
            RectTransform canvas
        )
        {
            _canvasRect = canvas;
            _target = transform;
            _damageText.text = $"{damage}";
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (_target == null) { return; }

            float offsetPosY = _target.transform.position.y + 0.1f;
            Vector3 offsetPos = new(_target.transform.position.x, offsetPosY, _target.transform.position.z);
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPoint, null, out Vector2 canvasPos);
            transform.localPosition = canvasPos;
        }
    }
}

