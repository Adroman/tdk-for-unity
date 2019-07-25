using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.EnemyData.Instances;
using Scrips.Instances;
using Scrips.Modifiers;
using Scrips.Modifiers.Stats;
using Scrips.Modifiers.Towers;
using Scrips.Priorities;
using Scrips.Towers.Specials;
using Scrips.Variables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scrips.Towers.BaseData
{
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class TowerInstance : MonoBehaviour
    {
        public string Name;

        public FloatModifiableStat MinDamage;
        public FloatModifiableStat MaxDamage;
        public FloatModifiableStat FiringSpeed;
        public FloatModifiableStat Range;
        public IntModifiableStat NumberOfTargets;

        public float Damage =>
            MaxDamage.Value > MinDamage.Value
            ? Random.Range(MinDamage.Value, MaxDamage.Value)
            : MaxDamage.Value;

        public float ActualFiringSpeed
        {
            get => FiringSpeed.Value;
            set
            {
                FiringSpeed.Value = value;
                _cooldown = 1 / FiringSpeed.Value;
            }
        }

        public float ActualRange
        {
            get
            {
                var colliderLocal = GetComponent<CircleCollider2D>();
                if (colliderLocal != null)
                {
                    colliderLocal.radius = Range.Value;
                    SetupCircle();
                }

                return Range.Value;
            }
            set
            {
                Range.Value = value;
                var colliderLocal = GetComponent<CircleCollider2D>();
                if (colliderLocal == null) return;
                colliderLocal.radius = Range.Value;
                SetupCircle();
            }
        }

        public BasePriority[] Priorities;
        public BaseFilter[] Filters;
        public GameObject BulletPrefab;
        public Transform RotationPoint;
        private bool _hasRotatingPoint;
        public Transform ShootingPoint;

        public List<TowerUpgradeNode> Upgrades = new List<TowerUpgradeNode>();

        public ModifierController ModifierController;

        public TowerCollection TowerCollection;

        private LineRenderer _circleRenderer;
        private float _cooldown;
        private float _cooldownLeft;

        private readonly List<TowerUpgradeNode> _appliedUpgrades = new List<TowerUpgradeNode>();
        private readonly List<TowerUpgradeNode> _upgradesLeft = new List<TowerUpgradeNode>();

        private List<EnemyInstance> _enemiesInRange;

        private static GameObject _bulletsParent;

        private SpecialComponent[] _specialComponents;

        private static GameObject BulletsParent
        {
            get
            {
                if (_bulletsParent == null)
                    _bulletsParent = GameObject.Find("Bullets");
                return _bulletsParent;
            }
        }

        private void Awake()
        {
            MinDamage = new FloatModifiableStat();
            MaxDamage = new FloatModifiableStat();
            FiringSpeed = new FloatModifiableStat();
            Range = new FloatModifiableStat();
            NumberOfTargets = new IntModifiableStat();
            _circleRenderer = GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            TowerCollection.AddInstance(this);
        }

        private void OnDisable()
        {
            TowerCollection.RemoveInstance(this);
        }

        // Use this for initialization
        private void Start()
        {
            //ActualRange = Range;

            //_cooldown = 1 / ActualFiringSpeed;
            _cooldownLeft = 0;

            _enemiesInRange = new List<EnemyInstance>();
            _upgradesLeft.AddRange(Upgrades);

            Filters = Filters.Where(f => f != null).ToArray();
            _hasRotatingPoint = RotationPoint != null;
            _specialComponents = GetComponents<SpecialComponent>();
        }

        // Update is called once per frame
        private void Update()
        {
            //var enemies = GameObject.FindObjectsOfType<Scrips.EnemyData.Instances.EnemyInstance>();

            // cooldown
            if (_cooldownLeft > 0)
            {
                _cooldownLeft -= Time.deltaTime;
                return;
            }

            var targets = _enemiesInRange.Where(t => t != null);
            // = enemies
            // .Select(e => new KeyValuePair<Scrips.EnemyData.Instances.EnemyInstance, float>(e, (e.transform.position - transform.position).magnitude))
            // .Where(e => e.Value <= ActualRange)
            // .Select(e => e.Key);

            foreach (var filter in Filters)
            {
                targets = filter.FilterEnemies(targets);
            }

            //var target = targets
            //    .OrderBy(e => e.DistanceToGoal.Key).ThenBy(e => e.DistanceToGoal.Value)
            //    .FirstOrDefault();

            foreach (var priority in Priorities)
            {
                targets = priority.Prioritize(targets);
            }

            var finalTargets = targets.Take(NumberOfTargets.Value).ToArray();

            if (finalTargets.Length == 0)    // no valid target
            {
                _cooldownLeft = 0;
                return;
            }

            RotateTurret(finalTargets.Select(t => t.transform));
            foreach (var target in finalTargets)
            {
                Fire(target);
            }
            _cooldownLeft = _cooldown + _cooldownLeft;
        }

        private void RotateTurret(IEnumerable<Transform> targets)
        {
            if (!_hasRotatingPoint) return; // do not rotate

            var vectorsToTarget = targets.Select(t => t.position - transform.position).ToArray();

            var vectorToTarget = new Vector3(vectorsToTarget.Average(v => v.x), vectorsToTarget.Average(v => v.y));
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            var q = Quaternion.AngleAxis(-angle, Vector3.up);
            RotationPoint.localRotation = Quaternion.Slerp(RotationPoint.rotation, q, 10000 * Time.deltaTime);
        }

        private Transform ShootingPointPosition => ShootingPoint != null ? ShootingPoint : transform;

        private void Fire(EnemyInstance target)
        {
            var shootingPoint = ShootingPointPosition;

            var bullet = PoolManager.Spawn(BulletPrefab, shootingPoint.position, transform.rotation, BulletsParent.transform).GetComponent<BulletInstance>();
            bullet.Target = target;
            bullet.Damage = Damage;
            foreach (var component in _specialComponents)
            {
                var newComponent = (SpecialComponent)bullet.gameObject.AddComponent(component.GetType());
                component.CopyDataToTargetComponent(newComponent);
            }
            //bullet.SpecialEffect = SpecialEffect; TODO: change it

        }

        public void Upgrade(TowerUpgradeNode upgrade)
        {
            if (!_upgradesLeft.Contains(upgrade))
            {
                Debug.LogError("Invalid upgrade");
                return;
            }

            if (!upgrade.Price.All(p => p.HasEnough()))
            {
                Debug.Log("Not enough resources to upgrade");
                return;
            }

            upgrade.Price.ForEach(p => p.Subtract());
            _upgradesLeft.Remove(upgrade);

            Name = string.IsNullOrWhiteSpace(upgrade.NewName) ? Name : upgrade.NewName;
            MinDamage.Value = upgrade.MinAtkIncrease.Apply(MinDamage.BaseValue);
            MaxDamage.Value = upgrade.MaxAtkIncrease.Apply(MaxDamage.BaseValue);
            ActualRange = upgrade.RangeIncrease.Apply(Range.BaseValue);
            ActualFiringSpeed = upgrade.FiringSpeedIncrease.Apply(FiringSpeed.BaseValue);
            foreach (var special in upgrade.SpecialIncreases)
            {
                var c = special.SpecialType.GetSpecialComponent(gameObject);
                if (c == null)
                {
                    Debug.LogWarning("Component not found");
                    continue;
                }

                special.Upgrade(c);
            }

            _appliedUpgrades.Add(upgrade);
        }

        public List<TowerUpgradeNode> GetPossibleUpgrades()
        {
            var result = new List<TowerUpgradeNode>();

            foreach (var upgrade in _upgradesLeft)
            {
                bool requirementsAllow = false;
                bool exclusionsAllow = false;

                if (upgrade.Requirements.Count == 0)
                {
                    // does it require any previous upgrade?
                    requirementsAllow = true;
                }
                else if (upgrade.RequirementAmount.MeetsTheRequirement(upgrade.Requirements, _appliedUpgrades))
                {
                    // does it have required upgrades?
                    requirementsAllow = true;
                }

                if (upgrade.Exclusions.Count == 0)
                {
                    // no exclusions
                    exclusionsAllow = true;
                }
                else if (!upgrade.ExclusionAmount.MeetsTheRequirement(upgrade.Exclusions, _appliedUpgrades))
                {
                    // is it blocked by other upgrades?
                    exclusionsAllow = true;
                }

                if (requirementsAllow && exclusionsAllow) result.Add(upgrade);
            }

            return result;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.gameObject.GetComponent<EnemyInstance>();
            if (enemy != null)
            {
                //Debug.Log("enemy entered");
                _enemiesInRange.Add(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var enemy = other.gameObject.GetComponent<EnemyInstance>();
            if (enemy != null)
            {
                _enemiesInRange.Remove(enemy);
                //Debug.Log("enemy exited");
            }
        }

        private void SetupCircle()
        {
            if (_circleRenderer == null)
            {
                return;
            }
            int vertices = 100;
            float radius = Range.Value;
            float deltaTheta = 2 * Mathf.PI / vertices;
            float theta = 0;

            _circleRenderer.positionCount = vertices;
            _circleRenderer.loop = true;
            for (int i = 0; i < vertices; i++)
            {
                var pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0);
                _circleRenderer.SetPosition(i, pos);
                theta += deltaTheta;
            }
        }

        public void ShowRangeCircle()
        {
            if (_circleRenderer == null) return;
            _circleRenderer.enabled = true;
        }

        public void HideRangeCircle()
        {
            if (_circleRenderer == null) return;
            _circleRenderer.enabled = false;
        }
    }
}
