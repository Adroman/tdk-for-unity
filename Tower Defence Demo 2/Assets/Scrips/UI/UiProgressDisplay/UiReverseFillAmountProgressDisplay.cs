using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI.UiProgressDisplay
{
    [RequireComponent(typeof(Image))]
    public class UiReverseFillAmountProgressDisplay : UiBaseProgress
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public override void UpdateValue(float amount)
        {
            _image.fillAmount = 1 - amount;
        }
    }
}