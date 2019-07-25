using System;
using Scrips.UI.UiTextDisplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scrips.UI.UiSkills
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public abstract class UiMouseDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public BaseUiTextDisplay TextDisplay;
        public Color ErrorColorToDisplay;
        public UiSkill Skill;

        protected Button Button;
        protected string Description;

        private void Awake()
        {
            Button = GetComponent<Button>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Button.enabled)
            {
                // nothing
            }
            else
            {
                Description = TextDisplay.GetText();
                TextDisplay.SetColor(ErrorColorToDisplay);
                TextDisplay.Display(BuildErrorText());
            }
        }

        public void EnterAgain()
        {
            OnPointerEnter(null);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Skill.UpdateTexts();
        }

        protected abstract string BuildErrorText();
    }
}