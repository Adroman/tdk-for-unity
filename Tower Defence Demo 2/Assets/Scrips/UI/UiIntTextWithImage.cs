using Scrips.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiIntTextWithImage : MonoBehaviour
    {
        public Text TextToUpdate;

        public Image ImageToUpdate;

        public void InitValue(IntCurrency fromCurrency)
        {
            TextToUpdate.text = fromCurrency.Amount.ToString();
            TextToUpdate.color = fromCurrency.Variable.IconColor;
            ImageToUpdate.sprite = fromCurrency.Variable.Icon;
            ImageToUpdate.color = fromCurrency.Variable.IconColor;
        }
    }
}