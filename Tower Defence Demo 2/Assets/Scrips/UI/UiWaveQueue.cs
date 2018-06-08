using Scrips.Waves;
using UnityEngine;

namespace Scrips.UI
{
    public class UiWaveQueue : MonoBehaviour
    {
        public UiWave UiWavePrefab;

        public UiWaveCluster UiWaveClusterPrefab;

        public void SpawnWave(Wave wave)
        {
            var uiWave = Instantiate(UiWavePrefab, transform);
            foreach (var cluster in wave.WaveClusters)
            {
                var uiCluster = Instantiate(UiWaveClusterPrefab, uiWave.transform);
                uiCluster.AmountIndicator.text = cluster.Amount.ToString();
                uiCluster.ImageIndicator.sprite = cluster.Prefab.Sprite;
                uiCluster.ClusterIndicator.SetUp(cluster);
                uiCluster.ClusterIndicator.gameObject.SetActive(false);
            }
        }

        public void DespawnTopWave()
        {
            if (transform.childCount > 0) transform.GetChild(0).GetComponent<UiWave>().Despawn();
        }
    }
}