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
        
        public void SetUp(WaveCluster cluster)
        {
            TextAmount.text = $"{cluster.Amount} {(cluster.Amount == 1 ? "Monster" : "Monsters")}";
            TextHp.text = $"Hitpoints: {cluster.EnemyData.InitialHitpoints}";
            TextArmor.text = $"Armor: {cluster.EnemyData.InitialArmor}";
            TextSpeed.text = $"Speed: {cluster.EnemyData.InitialSpeed}";
        }
    }
}