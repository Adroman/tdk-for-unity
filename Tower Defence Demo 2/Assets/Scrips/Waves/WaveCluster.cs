using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    [ExecuteInEditMode]
    public class WaveCluster : MonoBehaviour
    {
        // cluster info
        public int Amount;
        public float Interval;
        public float InitialCountDown;
        public bool SpawnWithPreviousCluster;

        [Range(0, 1)]
        public float IntervalDeviation;

        public List<Transform> OverrideSpawnpoints;

        // Enemy info
        public GameObject Prefab;

        private GameObject _sampleEnemy;

        public GameObject SampleEnemy
        {
            get
            {
                if (_sampleEnemy == null && Prefab != null)
                {
                    if (gameObject.transform.childCount == 0)
                    {
                        _sampleEnemy = Instantiate(Prefab, transform.position, transform.rotation, transform);
                        _sampleEnemy.SetActive(false);
                    }
                    else
                    {
                        _sampleEnemy = gameObject.transform.GetChild(0).gameObject;
                    }
                }

                return _sampleEnemy;
            }
            set
            {
                if (_sampleEnemy != null)
                {
                    DestroyImmediate(_sampleEnemy);
                }

                _sampleEnemy = Instantiate(value, transform.position, transform.rotation, transform);
                _sampleEnemy.SetActive(false);
            }
        }
    }
}

