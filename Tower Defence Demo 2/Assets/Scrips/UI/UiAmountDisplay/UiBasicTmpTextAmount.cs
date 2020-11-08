using TMPro;
using UnityEngine;

namespace Scrips.UI.UiAmountDisplay
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UiBasicTmpTextAmount : UiBaseAmount
    {
        private TextMeshProUGUI _text;

        public void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        public override void UpdateValue(int amount)
        {
            _text.text = amount.ToString();
        }
    }
}