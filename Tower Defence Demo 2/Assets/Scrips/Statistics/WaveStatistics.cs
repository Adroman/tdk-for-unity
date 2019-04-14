using System.Collections.Generic;
using System.Linq;
using Scrips.EnemyData.Instances;
using Scrips.Variables;
using Scrips.Waves;
using UnityEngine;

namespace Scrips.Statistics
{
    public class WaveStatistics : MonoBehaviour
    {
        public IntVariable WavesCalledEarly;

        public IntVariable WavesBeaten;

        private readonly Dictionary<int, int> _enemiesRemainingInWaves = new Dictionary<int, int>();

        public void WaveActivated(Wave wave)
        {
            if (wave.CalledEarly)
            {
                if (WavesCalledEarly != null) WavesCalledEarly.Value++;
            }

            if (_enemiesRemainingInWaves.ContainsKey(wave.WaveNumber))
            {
                Debug.LogWarning("This wave is already present");
            }

            _enemiesRemainingInWaves[wave.WaveNumber] = wave.WaveClusters.Sum(cluster => cluster.Amount);
        }

        public void EnemyKilled (EnemyInstance target)
        {
            int amountLeft;

            if (!_enemiesRemainingInWaves.TryGetValue(target.WaveNumber, out amountLeft))
            {
                Debug.LogWarning("This enemy has an invalid wave number");
                return;
            }

            if (--amountLeft == 0)
            {
                if (WavesBeaten != null) WavesBeaten.Value++;
                _enemiesRemainingInWaves.Remove(target.WaveNumber);
            }
            else
            {
                _enemiesRemainingInWaves[target.WaveNumber] = amountLeft;
            }
        }
    }
}