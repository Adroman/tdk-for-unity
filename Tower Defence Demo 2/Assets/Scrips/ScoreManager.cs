using Scrips.EnemyData.Instances;
using Scrips.Events;
using Scrips.Instances;
using Scrips.Variables;
using UnityEngine;


namespace Scrips
{
    public class ScoreManager : MonoBehaviour
    {
        public int InitialGold;
        public int InitialLives;

        public IntVariable WaveIndex;
        public IntVariable GoldAmount;
        public IntVariable LivesAmount;

        public GameEvent OnWaveIndexChanged;
        public GameEvent OnGoldAmountChanged;
        public GameEvent OnLivesAmountChanged;

        public static ScoreManager Instance { get; private set; }

        public int Gold
        {
            get { return GoldAmount.Value; }
            set
            {
                GoldAmount.Value = value;
            }
        }

        public int Lives
        {
            get { return LivesAmount.Value; }
            set
            {
                LivesAmount.Value = value;
            }
        }

        public int Wave
        {
            get { return WaveIndex.Value; }
            set
            {
                WaveIndex.Value = value;
            }
        }

        public int EnemiesRemaining { get; set; }
        public bool AllWavesDone { get; set; }

        // Use this for initialization
        void Start ()
        {
            Instance = this;
            Gold = InitialGold;
            Lives = InitialLives;
            Wave = 1;
            EnemiesRemaining = 0;
        }

        public void CheckVictory()
        {
            if(AllWavesDone && EnemiesRemaining == 0)
            {
                Debug.Log("--------VICTORY--------");
            }
        }

        public void GameOver()
        {
            Debug.Log("--------DEFEAT--------");
            Time.timeScale = 0;
        }

        public void CheckLives()
        {
            if (LivesAmount.Value <= 0) GameOver();
        }

        public void ReduceEnemyCount()
        {
            EnemiesRemaining--;
            CheckVictory();
        }

        public void AddLoot(EnemyInstance target)
        {
            foreach (var loot in target.IntLoot)
            {
                loot.Add();
            }
        }
    }
}
