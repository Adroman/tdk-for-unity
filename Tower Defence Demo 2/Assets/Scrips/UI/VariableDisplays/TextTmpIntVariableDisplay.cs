using Scrips.Variables;
using TMPro;
using UnityEngine;

namespace Scrips.UI.VariableDisplays
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextTmpIntVariableDisplay : MonoBehaviour
    {
        public IntVariable VariableToDisplay;

        public string Prefix;

        private TextMeshProUGUI _uiTextToUse;

        private void OnEnable()
        {
            _uiTextToUse = GetComponent<TextMeshProUGUI>();

            if (VariableToDisplay == null)
            {
                Debug.LogError($"Variable is not assigned to this component: {gameObject.name}, {typeof(TextIntVariableDisplay).Name}");
            }

            if (_uiTextToUse == null)
            {
                Debug.LogError($"UI Text is not assigned to this component: {gameObject.name}, {typeof(TextIntVariableDisplay).Name}");
            }

            if (VariableToDisplay != null && _uiTextToUse != null) UpdateText();
        }

        public void UpdateText()
        {
            _uiTextToUse.text = string.IsNullOrEmpty(Prefix)
                ? VariableToDisplay.Value.ToString()
                : $"{Prefix} {VariableToDisplay.Value.ToString()}";
        }
    }
}