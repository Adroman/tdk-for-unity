using Scrips.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scrips
{
    [CreateAssetMenu(menuName = "Tower defense kit/Level Configuration")]
    public class LevelConfiguration : ScriptableObject
    {
        [HideInInspector]
        public string TargetScenePath;

        public IntCurrency[] StartingResources;

        private void m()
        {
            SceneManager.LoadScene(TargetScenePath);
        }
    }
}