using Scrips.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiCurrencyIntTextWithImage : MonoBehaviour
    {
        public IntVariable Variable;

        public Image ImageToUse;

        public Text TextToUse;

        public TextMeshProUGUI TmpTextToUse;

        public string Prefix;

        public string Postfix;

        private void Start()
        {
            if (ImageToUse != null)
            {
                ImageToUse.sprite = Variable.Icon;
                ImageToUse.color = Variable.IconColor;
            }

            if (TmpTextToUse != null)
            {
                UpdateTmpText();
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

        public void UpdateTmpText()
        {
            TmpTextToUse.text = $"{Prefix}{Variable.Value}{Postfix}";
        }
    }
}