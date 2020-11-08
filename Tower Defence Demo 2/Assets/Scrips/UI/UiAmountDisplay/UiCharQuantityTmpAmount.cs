using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI.UiAmountDisplay
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UiCharQuantityTmpAmount : UiBaseAmount
    {
        private TextMeshProUGUI _text;

        public char CharacterToDisplay;

        public void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        public override void UpdateValue(int amount)
        {
            _text.text = new string(CharacterToDisplay, amount);
        }
    }
}