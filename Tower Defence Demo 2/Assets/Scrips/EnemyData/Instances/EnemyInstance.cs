using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Attributes;
using Scrips.BuffData;
using Scrips.Data;
using Scrips.EnemyData.Triggers;
using Scrips.Extensions;
using Scrips.Modifiers.Currency;
using Scrips.Modifiers.Stats;
using Scrips.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.EnemyData.Instances
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BaseTriggers))]
    public class EnemyInstance : MonoBehaviour
    {
        public List<EnemyAttribute> Attributes;

        public float Hitpoints;
        public float Armor;
        public float Speed;

        public List<ModifiedCurrency> IntLoot;
        public List<ModifiedCurrency> IntPunishments;

        public Image HealthImage;

        public Sprite Sprite;

        public EnemyCollection RuntimeCollection;

        public List<BaseBuffData> ActiveDebuffs = new List<BaseBuffData>();

        private TdTile _target;
        private Transform _spawnpoint;
        private Rigidbody2D _rigidbody;

        //[NonSerialized]
        public FloatModifiableStat InitialHitpoints;

        //[NonSerialized]
        public FloatModifiableStat InitialArmor;

        //[NonSerialized]
        public FloatModifiableStat InitialSpeed;

        public int WaveNumber;

        protected BaseTriggers TriggersComponent;        // we need this for sure

        private TdTile[] _tiles;

        public float DistanceToGoal => _target.GetComponent<TdTile>().DistanceToGoal + (transform.position - _target.transform.position).magnitude;

        private void Awake()
        {
            _tiles = GameObject.Find("Tiles").GetComponentsInChildren<TdTile>();
            InitialHitpoints = new FloatModifiableStat();
            InitialArmor = new FloatModifiableStat();
            InitialSpeed = new FloatModifiableStat();
        }

        private void OnEnable()
        {
            if (RuntimeCollection != null)
            {
                RuntimeCollection.AddInstance(this);
            }
        }

        private void OnDisable()
        {
            if (RuntimeCollection != null)
            {
                RuntimeCollection.RemoveInstance(this);
            }
        }

        // Use this for initialization
        private void Start ()
        {
            TriggersComponent = GetComponent<BaseTriggers>();
            _rigidbody = GetComponent<Rigidbody2D>();

            var firstTargets = FindNearestTile(_tiles).NextTiles;
            _target = firstTargets[GetRandom(0, firstTargets.Count)];

            ResetStats();
        }

        private void ResetStats()
        {
            Speed = InitialSpeed.Value;
            Armor = InitialArmor.Value;
            Hitpoints = InitialHitpoints.Value;

            ActiveDebuffs.Clear();
        }

        // Update is called once per frame
        private void Update ()
        {
            // direction
            var thisPosition = transform.position;
            thisPosition.z = 0;
            var thatPosition = _target.transform.position;
            thatPosition.z = 0;

            var dir = thatPosition - thisPosition;

            // distance to target
            float distanceLeft = dir.magnitude;
            float distanceToTravel = Speed * Time.deltaTime;

            while(distanceLeft < distanceToTravel)
            {
                _rigidbody.MovePosition(transform.position + dir.normalized * distanceToTravel);
                //transform.Translate(dir.normalized * distanceToTravel, Space.World);
                distanceToTravel -= distanceLeft;

                if (_target.IsGoal)
                {
                    TargetReached();
                    return;
                }

                var nextTargets = _target.NextTiles;
                _target = nextTargets[GetRandom(0, nextTargets.Count)];

                thisPosition = transform.position;
                thisPosition.z = 0;
                thatPosition = _target.transform.position;
                thatPosition.z = 0;

                dir = thatPosition - thisPosition;
                distanceLeft = dir.magnitude;
            }

            foreach (var activeDebuff in ActiveDebuffs.ToArray())
            {
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
            if (TriggersComponent.HasOnFinishTrigger) TriggersComponent.OnFinish.Invoke(this);
        }

        public void Despawn(EnemyInstance target)
        {
            if (this == target)
                Destroy(gameObject);
        }

        public void TeleportToStart(EnemyInstance target)
        {
            if (this == target)
            {
                transform.position = new Vector3(_spawnpoint.position.x, _spawnpoint.position.y);
                SetSpawnPoint(_spawnpoint, true);
            }
        }

        public void SetSpawnPoint(Transform sp, bool move)
        {
            _spawnpoint = sp;
            var position = sp.position;
            if (move)
            {
                var firstTargets = FindNearestTile(_tiles).NextTiles;
                transform.position = new Vector3(position.x, position.y);
                _target = firstTargets[GetRandom(0, firstTargets.Count)];
            }
        }

        private TdTile FindNearestTile(TdTile[] tdTiles)
        {
            foreach (var tile in tdTiles)
            {
                var thisPosition = transform.position;
                thisPosition.z = 0;
                var thatPosition = tile.transform.position;
                thatPosition.z = 0;
                if ((thisPosition - thatPosition).magnitude < 0.0001f)
                {
                    return tile;
                }
            }
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation - this is a special case, which is not supposed to happen
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
            //ScoreManager.Instance.Lives -= amount;
            foreach (var punishment in IntPunishments)
            {
                punishment.Subtract();
            }
        }

        private void Die()
        {
            if (TriggersComponent.HasOnDeathTrigger) TriggersComponent.OnDeath.Invoke(this);

            Destroy(gameObject);
        }

        public void TakeDamage(float amount, bool ignoreArmor = false)
        {
            Hitpoints -= Math.Max(amount - (ignoreArmor ? 0 : Armor), 0);
            UpdateHealthbar();
            if (Hitpoints <= 0)
                Die();
        }

        private void UpdateHealthbar()
        {
            HealthImage.fillAmount = InitialHitpoints.Value > 0 ? Hitpoints / InitialHitpoints.Value : 0;
        }

        public void ReduceArmor(float amount)
        {
            Armor = Math.Max(0, Armor - amount);
        }

        // origin: https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers
        private static float GetRandomNumber(float minimum, float maximum, System.Random rand = null)
        {
            if (rand == null) rand = new System.Random();
            return (float)rand.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
