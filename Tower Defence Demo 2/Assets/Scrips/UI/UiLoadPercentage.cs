using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiLoadPercentage : MonoBehaviour
    {
        private Text UiText;

        private void OnEnable()
        {
            UiText = GetComponent<Text>();
        }

        public void UpdateValue(float value)
        {
            int percentage = Mathf.FloorToInt(value * 100);
            //Debug.Log(value);
            UiText.text = $"{percentage} %";
        }
    }
}