using Scrips.Towers.Specials;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    [RequireComponent(typeof(Text))]
    public class UiSpecialText : MonoBehaviour
    {
        private Text _text;

        private void Start()
        {
            _text = GetComponent<Text>();
        }

        public void UpdateText(SpecialType special)
        {
            _text.color = special.UiColor;
            _text.text = special.GetUiText();
        }

        public void UpdateText(SpecialComponent special)
        {
            _text.color = special.SpecialType.UiColor;
            _text.text = special.GetUiText();
        }
    }
}