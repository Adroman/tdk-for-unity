using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scrips
{
    [CreateAssetMenu(menuName = "Level Configuration")]
    public class LevelConfiguration : ScriptableObject
    {
        [HideInInspector]
        public string TargetScenePath;

        private void m()
        {
            SceneManager.LoadScene(TargetScenePath);
        }
    }
}