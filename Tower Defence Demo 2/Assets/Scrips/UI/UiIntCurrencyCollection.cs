using System.Collections.Generic;
using Scrips.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiIntCurrencyCollection : MonoBehaviour
    {
        public string Prefix;

        public Text PrefixText;

        public UiIntTextWithImage UiPrefab;

        public void Init(List<IntCurrency> listToUse)
        {
            if (PrefixText != null)
            {
                PrefixText.text = Prefix;
            }

            foreach (var intCurrency in listToUse)
            {
                var twi = Instantiate(UiPrefab, transform);
                twi.InitValue(intCurrency);
            }
        }
    }
}