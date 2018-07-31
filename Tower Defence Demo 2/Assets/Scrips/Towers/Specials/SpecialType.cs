using UnityEngine;

namespace Scrips.Towers.Specials
{
    public abstract class SpecialType : ScriptableObject
    {
        public abstract SpecialComponent GetOrCreateSpecialComponent(GameObject go);

        public abstract SpecialComponent GetSpecialComponent(GameObject go);

        public abstract string GetUiText();

        public Color UiColor;
    }
}