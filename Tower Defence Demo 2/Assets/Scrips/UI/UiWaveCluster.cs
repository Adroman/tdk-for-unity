using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiWaveCluster : MonoBehaviour
    {
        public Text AmountIndicator;

        public Image ImageIndicator;

        public UiClusterInfo ClusterIndicator;

        public void MouseEnter()
        {
            ClusterIndicator.gameObject.SetActive(true);
        }

        public void MouseExit()
        {
            ClusterIndicator.gameObject.SetActive(false);
        }
    }
}