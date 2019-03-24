using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI.UiProgressDisplay
{
    [RequireComponent(typeof(Image))]
    public class UiFillAmountProgressDisplay : UiBaseProgress
    {
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        public override void UpdateValue(float amount)
        {
            _image.fillAmount = amount;
        }
    }
}