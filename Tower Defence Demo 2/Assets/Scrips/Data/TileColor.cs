using UnityEngine;

namespace Scrips.Data
{
    [CreateAssetMenu]
    public class TileColor : ScriptableObject
    {
        public Color EditorColor;

        public Color InGameColor;

        public Color InGameHoverColor;
    }
}