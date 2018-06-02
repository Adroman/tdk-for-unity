using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scrips.CustomTypes;
using Scrips.Events;
using Scrips.Events.Enemies;
using Scrips.Utils;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Waves
{
    public class WaveController : MonoBehaviour
    {
        [HideInInspector]
        public List<Wave> Waves;

        [HideInInspector]
        [SerializeField]
        private WaveGenerator _generator;

        public LogLevel LogLevel;

        public GameEvent OnWaveStarted;

        public EnemyEvent OnEnemySpawned;

        public IntVariable EnemiesWaiting;

        public WaveGenerator Generator
        {
            get
            {
                if (_generator == null) _generator = new WaveGenerator();
                return _generator;
            }
        }

        //public Button CallEarly;

        private int _currentWave;


        // Use this for initialization
        void Start ()
        {
            ScoreManager.Instance.Wave = -2;
            //var button = CallEarly.GetComponent<Button>();
            //button.onClick.AddListener(NextWave);
            _currentWave = -2;
            ScoreManager.Instance.Wave = Math.Max(0, _currentWave);
            //ActivateWaves();
            if (EnemiesWaiting != null) EnemiesWaiting.Value = 0;

            NextWave();
        }

        private IEnumerator ActivateWave()
        {
            int currentWave = _currentWave;
            if (currentWave < Waves.Count)
            {
                DebugUtils.LogDebug(LogLevel, "Activating wave " + (currentWave + 2));
                if (currentWave > -1)
                {
                    Waves[currentWave].ActivateWave();
                    if (EnemiesWaiting != null)
                    {
                        EnemiesWaiting.Value += Waves[currentWave].WaveClusters.Sum(cluster => cluster.Amount);
                    }
                }
                DebugUtils.LogDebug(LogLevel, "Waiting for wave " + (currentWave + 2));
                if (currentWave + 1 < Waves.Count) yield return new WaitForSeconds(Waves[currentWave + 1].Countdown);
                if (currentWave == _currentWave)
                {
                    if (OnWaveStarted != null) OnWaveStarted.Invoke();
                    NextWave();  // no wave was called
                }
                DebugUtils.LogDebug(LogLevel, "Done wave " + (currentWave + 2));

            }
        }

        public void NextWave()
        {
            _currentWave++;
            StartCoroutine(ActivateWave());
            ScoreManager.Instance.Wave = Math.Max(0, _currentWave);
        }

        public void CallWaveEarly()
        {
            if (OnWaveStarted != null) OnWaveStarted.Invoke();
            NextWave();
        }

        public void SubstractOneWaitingEnemy()
        {
            if (EnemiesWaiting != null) EnemiesWaiting.Value--;
            else Debug.LogWarning("No variable for representing enemies waiting.");
        }
    }
}
