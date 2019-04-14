using Scrips.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI.VariableDisplays
{
    public class TextIntVariableDisplay : MonoBehaviour
    {
        public IntVariable VariableToDisplay;

        public string Prefix;

        public Text UiTextToUse;

        private void OnEnable()
        {
            if (VariableToDisplay == null)
            {
                Debug.LogError($"Variable is not assigned to this component: {gameObject.name}, {typeof(TextIntVariableDisplay).Name}");
            }

            if (UiTextToUse == null)
            {
                Debug.LogError($"UI Text is not assigned to this component: {gameObject.name}, {typeof(TextIntVariableDisplay).Name}");
            }
            
            if (VariableToDisplay != null && UiTextToUse != null) UpdateText();
        }

        public void UpdateText()
        {
            UiTextToUse.text = string.IsNullOrEmpty(Prefix)
                ? VariableToDisplay.Value.ToString()
                : $"{Prefix} {VariableToDisplay.Value.ToString()}";
        }
    }
}