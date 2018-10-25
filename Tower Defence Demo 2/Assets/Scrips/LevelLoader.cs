using System.Collections;
using Scrips.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scrips
{
    public class LevelLoader : MonoBehaviour
    {
        public Slider Slider;

        public UiLoadPercentage LoadPercentage;

        public void LoadLevel(string levelPath)
        {
            StartCoroutine(LoadAsync(levelPath));
        }

        private IEnumerator LoadAsync(string levelPath)
        {
            yield return null;
            var operation = SceneManager.LoadSceneAsync(levelPath);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                if (Slider != null) Slider.value = progress;

                if (LoadPercentage != null) LoadPercentage.UpdateValue(progress);

                yield return null;
            }
        }
    }
}