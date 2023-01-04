using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityIcon : MonoBehaviour
{
    public Image CooldownImage;
    //public TextMeshProUGUI CooldownText;
    public float ImageSize = 34f;

    private Image _abilityImage;

    private Func<float> _GetOverallCooldown;
    private Func<float> _GetCurentCooldown;

    private bool _isInit = false;

    public void InitIcon(
        Image image,
        Func<float> getOverallCooldown,
        Func<float> getCurentCooldown
    )
    {
        _abilityImage = image;
        CooldownImage.rectTransform.sizeDelta = _abilityImage.rectTransform.sizeDelta = new Vector2(ImageSize, ImageSize);
        _GetOverallCooldown = getOverallCooldown;
        _GetCurentCooldown = getCurentCooldown;
        _isInit = true;
    }

    private void Update()
    {
        if (!_isInit) { return; }

        var cooldownText = Mathf.RoundToInt(_GetCurentCooldown()).ToString();
        //CooldownText.SetText(cooldownText);
        CooldownImage.fillAmount = _GetCurentCooldown() / _GetOverallCooldown();

    }
}

