﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Scrips.BuffData;
using Scrips.Data;
using Scrips.EnemyData.AutoGenerateModifers;
using Scrips.EnemyData.Triggers;
using Scrips.EnemyData.WaveData;
using Scrips.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.EnemyData.Instances
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BaseTriggers))]
    [RequireComponent(typeof(BaseWaveData))]
    [RequireComponent(typeof(BaseAutoGenerateModifiers))]
    public class EnemyInstance : MonoBehaviour
    {
        public float Hitpoints;
        public float Armor;
        public float Speed;
        public List<IntCurrency> IntLoot;
        public List<IntCurrency> IntPunishments;

        public GameObject waypointType;

        public Image HealthImage;

        public List<BaseBuffData> ActiveDebuffs;

        private GameObject _target;
        private Transform _spawnpoint;
        private Rigidbody2D _rigidbody;

        private float _initialHitpoint;
        private float _initialArmor;
        [HideInInspector]
        [NonSerialized]
        public float InitialSpeed;

        protected BaseTriggers TriggersComponent;        // we need this for sure
        protected BaseWaveData WaveDataComponent;        // not sure if we need this

        public float DistanceToGoal => _target.GetComponent<TdTile>().DistanceToGoal + (transform.position - _target.transform.position).magnitude;

        // Use this for initialization
        private void Start ()
        {
            TriggersComponent = GetComponent<BaseTriggers>();
            WaveDataComponent = GetComponent<BaseWaveData>();

            _rigidbody = GetComponent<Rigidbody2D>();

            ResetStats();

            var a = GameObject.Find("Level/Tiles");
            var b = a.GetComponentsInChildren<TdTile>();
            var firstTargets = FindNearestTile(b).NextTiles;
            _target = firstTargets[GetRandom(0, firstTargets.Count)].gameObject;
        }

        // Update is called once per frame
        private void Update ()
        {
            // direction
            var thispos = transform.position;
            thispos.z = 0;
            var thatpos = _target.transform.position;
            thatpos.z = 0;

            var dir = thatpos - thispos;

            // distance to target
            float distanceLeft = dir.magnitude;
            float distanceToTravel = Speed * Time.deltaTime;

            while(distanceLeft < distanceToTravel)
            {
                _rigidbody.MovePosition(transform.position + dir.normalized * distanceToTravel);
                //transform.Translate(dir.normalized * distanceToTravel, Space.World);
                distanceToTravel -= distanceLeft;

                if (_target.GetComponent<TdTile>().IsGoal)
                {
                    TargetReached();
                    return;
                }

                var nextTargets = _target.GetComponent<TdTile>().NextTiles;
                _target = nextTargets[GetRandom(0, nextTargets.Count)].gameObject;

                thispos = transform.position;
                thispos.z = 0;
                thatpos = _target.transform.position;
                thatpos.z = 0;

                dir = thatpos - thispos;
                distanceLeft = dir.magnitude;
            }

            foreach (var activeDebuff in ActiveDebuffs.ToArray())
            {
                if (!activeDebuff.IsActive) ActiveDebuffs.Remove(activeDebuff);

                activeDebuff.Update(Time.deltaTime);
            }

            // rotation
            var vectorToTarget = dir;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            var q = Quaternion.AngleAxis(angle, Vector3.forward);
            var rotatingPart = transform.GetChild(0);
            rotatingPart.rotation = Quaternion.Slerp(rotatingPart.rotation, q, 10000 * Time.deltaTime);

            // movement
            //transform.Translate(dir.normalized * distanceToTravel, Space.World);
            _rigidbody.MovePosition(transform.position + dir.normalized * distanceToTravel);
        }

        private void TargetReached()
        {
            // take life, drain mana, grab item
            if (--ScoreManager.Instance.Lives <= 0)
            {
                // Game over
            }

            // destroy
            Destroy(gameObject);
        }

        public void SetSpawnPoint(Transform sp, bool move)
        {
            _spawnpoint = sp;
            if (move)
            {
                var a = GameObject.Find("Level/Tiles");
                var b = a.GetComponentsInChildren<TdTile>();
                var firstTargets = FindNearestTile(b).NextTiles;
                transform.position = new Vector3(sp.position.x, sp.position.y);
                _target = firstTargets[GetRandom(0, firstTargets.Count)].gameObject;
            }
        }

        private TdTile FindNearestTile(TdTile[] tdTiles)
        {
            foreach (var tile in tdTiles)
            {
                var thispos = transform.position;
                thispos.z = 0;
                var thatpos = tile.transform.position;
                thatpos.z = 0;
                if ((thispos - thatpos).magnitude < 0.0001f)
                {
                    return tile;
                }
            }
            Debug.LogError($"Invalid position {transform.position}");
            return null;
        }

        private int GetRandom(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public void DestroyInstance()
        {
            Destroy(this);
        }

        public void TakeLife(int amount)
        {
            ScoreManager.Instance.Lives -= amount;
            foreach (var punishment in IntPunishments)
            {
                punishment.Substract();
            }
        }

        private void Die()
        {
            if (TriggersComponent.OnDeath != null) TriggersComponent.OnDeath.Invoke(this);

            Destroy(gameObject);
        }

        public void TakeDamage(float amount)
        {
            Hitpoints -= Math.Max(amount - Armor, 0);
            UpdateHealthbar();
            // apply specialEffects
            if (Hitpoints <= 0)
                Die();
        }

        public void TakeCumulutativeDamage(float dps, bool ignoreArmor, float deltaTime)
        {
            float trueAmount = dps * deltaTime;

            if (ignoreArmor)
            {
                // magic with armor
            }

            Hitpoints -= trueAmount;
            UpdateHealthbar();
            if (Hitpoints <= 0)
                Die();
        }

        private void UpdateHealthbar()
        {
            HealthImage.fillAmount = _initialHitpoint > 0 ? Hitpoints / _initialHitpoint : 0;
        }

        public void ReduceArmor(float amount)
        {
            Armor = Math.Max(0, Armor - amount);
        }

        private void ResetStats()
        {
            ActiveDebuffs = new List<BaseBuffData>();
            _initialArmor = Armor = GetDeviatedValue(WaveDataComponent.InitialArmor, WaveDataComponent.ArmorDeviation);
            _initialHitpoint = Hitpoints = GetDeviatedValue(WaveDataComponent.InitialHitpoints, WaveDataComponent.HitpointsDeviation);
            InitialSpeed = Speed = GetDeviatedValue(WaveDataComponent.InitialSpeed, WaveDataComponent.SpeedDeviation);
            IntLoot = WaveDataComponent.IntLoot;
        }

        private float GetDeviatedValue(float baseValue, float deviation)
        {
            return baseValue * (GetRandomNumber(1 - deviation, 1 + deviation));
        }

        // origin: https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers
        private static float GetRandomNumber(float minimum, float maximum, System.Random rand = null)
        {
            if (rand == null) rand = new System.Random();
            return (float)rand.NextDouble() * (maximum - minimum) + minimum;
        }

        public void CalculateCurrentSpeed()
        {
            float minimumSpeed = InitialSpeed;
            foreach (var speedDebuff in ActiveDebuffs.OfType<SlownessBuffData>())
            {
                if (!speedDebuff.IsActive)
                {
                    ActiveDebuffs.Remove(speedDebuff);
                }

                float speedFromThisDebuff = InitialSpeed.Substract(speedDebuff.Amount);
                minimumSpeed = Mathf.Min(minimumSpeed, speedFromThisDebuff);
            }

            Speed = minimumSpeed;
        }
    }
}