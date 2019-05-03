using UnityEngine;

namespace Scrips.Data
{
    [CreateAssetMenu(menuName = "Tower defense kit/Tile Color")]
    public class TileColor : ScriptableObject
    {
        public Color EditorColor;

        public Color InGameColor;

        public Color InGameHoverColor;
    }
}