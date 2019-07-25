using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI.UiTextDisplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class UiTextDisplay : BaseUiTextDisplay
    {
        private Text _textToDisplayTo;

        private void Awake() => _textToDisplayTo = GetComponent<Text>();

        public override string GetText() => _textToDisplayTo.text;

        public override void Display(string text) => _textToDisplayTo.text = text;

        public override void SetColor(Color color) => _textToDisplayTo.color = color;
    }
}