using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI.UiAmountDisplay
{
    [RequireComponent(typeof(Text))]
    public class UiCharQuantityAmount : UiBaseAmount
    {
        private Text _text;

        public char CharacterToDisplay;

        public void Start()
        {
            _text = GetComponent<Text>();
        }

        public override void UpdateValue(int amount)
        {
            _text.text = new string(CharacterToDisplay, amount);
        }
    }
}