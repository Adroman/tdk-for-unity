using System.Runtime.Serialization.Formatters;
using Scrips.EnemyData.Instances;
using Scrips.Events;
using Scrips.Instances;
using Scrips.UI;
using Scrips.Variables;
using UnityEngine;

namespace Scrips
{
    public class ScoreManager : MonoBehaviour
    {
        public LevelConfiguration Configuration;

        public IntVariable WaveIndex;
        public IntVariable GoldAmount;
        public IntVariable LivesAmount;

        public IntVariable ActiveEnemies;
        public IntVariable WaitingEnemies;
        public BooleanVariable LastWave;

        public GameEvent OnWaveIndexChanged;
        public GameEvent OnGoldAmountChanged;
        public GameEvent OnLivesAmountChanged;

        public UiController Ui;

        // Use this for initialization
        private void Start ()
        {
            foreach (var resource in Configuration.StartingResources)
            {
                resource.SetToAmount();
            }
            WaveIndex.Value = 1;
        }

        public void CheckVictory()
        {
            if (LastWave.Value && ActiveEnemies.Value == 0 && WaitingEnemies.Value == 0)
            {
                Ui.SwitchToVictoryUi();
            }
        }

        public void GameOver()
        {
            Ui.SwitchToDefeatUi();
            Time.timeScale = 0;
        }

        public void CheckLives()
        {
            if (LivesAmount.Value <= 0) GameOver();
        }

        public void AddLoot(EnemyInstance target)
        {
            foreach (var loot in target.IntLoot)
            {
                loot.Add();
            }
        }

        public void PunishPlayer(EnemyInstance target)
        {
            foreach (var punishment in target.IntPunishments)
            {
                punishment.Subtract();
            }
        }
    }
}
