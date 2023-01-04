using UnityEngine;
using UnityEngine.UI;

namespace Eincode.ZombieSurvival.UI
{
    public class SliderBarBehaviour : MonoBehaviour
    {
        public Slider Slider;
        public Vector3 Offset;

        public virtual void SetValue(float value, float maxValue)
        {
            Slider.maxValue = maxValue;
            Slider.value = value;
        }
    }
}

