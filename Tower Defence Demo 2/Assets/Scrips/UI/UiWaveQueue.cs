using Scrips.Waves;
using UnityEngine;

namespace Scrips.UI
{
    public class UiWaveQueue : MonoBehaviour
    {
        public UiWave UiWavePrefab;

        public UiWaveCluster UiWaveClusterPrefab;

        private UiWave _waveToDespawn;

        public void SpawnWave(Wave wave, bool animate)
        {
            var uiWave = Instantiate(UiWavePrefab, transform);
            if (_waveToDespawn == null) _waveToDespawn = uiWave;

            foreach (var cluster in wave.WaveClusters)
            {
                var uiCluster = Instantiate(UiWaveClusterPrefab, uiWave.Layout);
                uiCluster.AmountIndicator.text = cluster.Amount.ToString();
                uiCluster.ImageIndicator.sprite = cluster.Prefab.Sprite;
                uiCluster.ClusterIndicator.SetUp(cluster);
                uiCluster.ClusterIndicator.gameObject.SetActive(false);
            }
            if (animate) uiWave.Spawn();
        }

        public void DespawnTopWave()
        {
            var wave = _waveToDespawn;
            int index = wave.transform.GetSiblingIndex();

            _waveToDespawn = transform.childCount >= index + 2
                ? transform.GetChild(index + 1).gameObject.GetComponent<UiWave>()
                : null;

            wave.Despawn();
        }
    }
}