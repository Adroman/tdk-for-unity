using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI.UiProgressDisplay
{
    [RequireComponent(typeof(Slider))]
    public class UiSliderProgress : UiBaseProgress
    {
        private Slider _slider;

        private void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.minValue = 0;
            _slider.maxValue = 1;
        }

        public override void UpdateValue(float amount)
        {
            _slider.value = amount;
        }
    }
}