using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI.UiAmountDisplay
{
    [RequireComponent(typeof(Text))]
    public class UiBasicTextAmount : UiBaseAmount
    {
        private Text _text;

        public void Start()
        {
            _text = GetComponent<Text>();
        }

        public override void UpdateValue(int amount)
        {
            _text.text = amount.ToString();
        }
    }
}