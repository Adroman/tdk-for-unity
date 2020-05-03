using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scrips.UI
{
    public class UiFader : MonoBehaviour
    {
        public Animator Animator;

        public LevelLoader LevelLoader;

        private string _levelToLoad;
        private static readonly int FadeOut = Animator.StringToHash("FadeOut");

        public void FadeToLevel(string levelName)
        {
            _levelToLoad = levelName;
            Animator.SetTrigger(FadeOut);
        }

        public void OnFadeComplete()
        {
            if (LevelLoader != null)
            {
                LevelLoader.LoadLevel(_levelToLoad);
            }
            else
            {
                SceneManager.LoadScene(_levelToLoad);
            }
        }
    }
}