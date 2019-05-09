using System.Runtime.Serialization.Formatters;
using Scrips.Data;
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
        public IntVariable Score;
        public BooleanVariable LastWave;

        public GameEvent OnWaveIndexChanged;
        public GameEvent OnGoldAmountChanged;
        public GameEvent OnLivesAmountChanged;

        public UiController Ui;

        private PlayerData _playerData;

        // Use this for initialization
        private void Start ()
        {
            foreach (var resource in Configuration.StartingResources)
            {
                resource.SetToAmount();
            }
            WaveIndex.Value = 1;
            _playerData = PlayerData.ActivePlayer;
        }

        public void CheckVictory()
        {
            if (LastWave.Value && ActiveEnemies.Value == 0 && WaitingEnemies.Value == 0)
            {
                Ui.SwitchToVictoryUi();
            }
        }

        public void ReturnToMap(bool victory)
        {
            if (victory)
            {
                var newScore = new IntCurrency
                {
                    Variable = _playerData.TotalScore.Variable,
                    Amount = Score.Value
                };

                var diff = _playerData.SetOrAddScore(Configuration, newScore);

                int playerLevel = _playerData.PlayerLevel;
                
                _playerData.TotalScore.Amount += diff.Amount;

                while (_playerData.TotalScore.Amount >= _playerData.ScoreFormula.GetLevelRequirement(playerLevel))
                {
                    playerLevel++;
                }

                _playerData.PlayerLevel = playerLevel;
            }
            Time.timeScale = 1;
            GetComponent<UiLevel>().LoadLevel();
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
