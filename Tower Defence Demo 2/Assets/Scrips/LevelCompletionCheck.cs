using System;
using System.Linq;
using UnityEngine;

namespace Scrips
{
    [RequireComponent(typeof(UiLevel))]
    public class LevelCompletionCheck : MonoBehaviour
    {
        public GameObject GameObjectToEnable;
        
        public void Start()
        {
            var levelToCheck = GetComponent<UiLevel>();
            var pd = levelToCheck.PlayerData;
            var configuration = levelToCheck.TargetLevel;

            if (pd.CompletedLevels.Any(l => l.Level == configuration))
            {
                GameObjectToEnable.SetActive(true);
            }
        }
    }
}