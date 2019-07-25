using System;
using TMPro;
using UnityEngine;

namespace Scrips.UI.UiTextDisplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UiTmpTextDisplay : BaseUiTextDisplay
    {
        private TextMeshProUGUI _textToDisplayTo;

        private void Awake() => _textToDisplayTo = GetComponent<TextMeshProUGUI>();

        public override string GetText() => _textToDisplayTo.text;

        public override void Display(string text) => _textToDisplayTo.text = text;

        public override void SetColor(Color color) => _textToDisplayTo.color = color;
    }
}