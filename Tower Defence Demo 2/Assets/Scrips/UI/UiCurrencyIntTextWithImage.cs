using Scrips.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiCurrencyIntTextWithImage : MonoBehaviour
    {
        public IntVariable Variable;

        public Image ImageToUse;

        public Text TextToUse;

        public string Prefix;

        public string Postfix;

        private void Start()
        {
            if (ImageToUse != null)
            {
                ImageToUse.sprite = Variable.Icon;
                ImageToUse.color = Variable.IconColor;
            }

            if (TextToUse != null)
            {
                UpdateText();
            }
        }

        public void UpdateText()
        {
            TextToUse.text = $"{Prefix}{Variable.Value}{Postfix}";
        }
    }
}