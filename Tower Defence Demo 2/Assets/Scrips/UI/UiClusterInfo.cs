using System.Globalization;
using Scrips.Waves;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiClusterInfo : MonoBehaviour
    {
        public Text TextAmount;
        public Text TextHp;
        public Text TextArmor;
        public Text TextSpeed;
        public UiIntCurrencyCollection Loot;
        public UiIntCurrencyCollection Punishments;
        public Text TextDifficulty;
        public Text TextDifficultyPerMonster;

        public void SetUp(WaveCluster cluster)
        {
            TextAmount.text = $"{cluster.Amount} {(cluster.Amount == 1 ? "Monster" : "Monsters")}";
            var enemyData = cluster.EnemyData;

            TextHp.text = $"Hitpoints: {GetEnemyStat(enemyData.InitialHitpoints, enemyData.HitpointsDeviation)}";
            TextArmor.text = $"Armor: {GetEnemyStat(enemyData.InitialArmor, enemyData.ArmorDeviation)}";
            TextSpeed.text = $"Speed: {GetEnemyStat(enemyData.InitialSpeed, enemyData.SpeedDeviation)}";
            TextDifficulty.text = $"Difficulty: {cluster.Difficulty}";
            TextDifficultyPerMonster.text = $"Difficulty: {cluster.DifficultyPerMonster}";
            Loot.Init(cluster.EnemyData.IntLoots);
            Punishments.Init(cluster.EnemyData.IntPunishments);
        }

        private string GetEnemyStat(float baseValue, float deviation)
        {
            float minValue = baseValue * (1 - deviation);
            float maxValue = baseValue * (1 + deviation);

            return maxValue - minValue < 0.01
                ? baseValue.ToString(CultureInfo.InvariantCulture)
                : $"{minValue:0.00} - {maxValue:0.00}";
        }
    }
}