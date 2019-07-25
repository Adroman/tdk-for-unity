using UnityEngine;

namespace Scrips.UI.UiTextDisplay
{
    public abstract class BaseUiTextDisplay : MonoBehaviour
    {
        public abstract string GetText();

        public abstract void Display(string text);

        public abstract void SetColor(Color color);

        public void DisplayFormat(string format, params object[] data)
        {
            Display(string.Format(format, data));
        }
    }
}