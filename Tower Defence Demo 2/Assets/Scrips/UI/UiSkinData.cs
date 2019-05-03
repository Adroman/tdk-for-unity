using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    [CreateAssetMenu(menuName = "Tower defense kit/UI/Skin data")]
    public class UiSkinData : ScriptableObject
    {
        public Sprite ButtonSprite;
        public SpriteState ButtonSpriteState;
    }
}