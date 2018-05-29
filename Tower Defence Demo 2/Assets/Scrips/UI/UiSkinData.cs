using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    [CreateAssetMenu(menuName = "UI/Skin data")]
    public class UiSkinData : ScriptableObject
    {
        public Sprite ButtonSprite;
        public SpriteState ButtonSpriteState;
    }
}