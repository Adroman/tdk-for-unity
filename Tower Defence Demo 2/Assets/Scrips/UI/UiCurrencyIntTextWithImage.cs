using System;
using Scrips.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiCurrencyIntTextWithImage : UITextWithImage<IntVariable, int>
    {
        [Obsolete("Use UpdateText instead")]
        public void UpdateTmpText()
        {
            TmpTextToUse.text = $"{Prefix}{Variable.Value}{Postfix}";
        }
    }
}