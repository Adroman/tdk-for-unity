using UnityEditor;
using UnityEngine;

namespace Scrips
{
    [CreateAssetMenu(menuName = "Player Data")]
    public class PlayerData : ScriptableObject
    {
        public int Counter = 0;
    }
}