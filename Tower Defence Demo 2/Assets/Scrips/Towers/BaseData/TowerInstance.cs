using System.Collections.Generic;
using System.Linq;
using Scrips.EnemyData.Instances;
using Scrips.Instances;
using Scrips.Priorities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scrips.Towers.BaseData
{
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class TowerInstance : MonoBehaviour
    {
        private float ActualRange
        {
            get { return Range; }
            set
            {
                Range = value;
                var colliderLocal = GetComponent<CircleCollider2D>();
                if (colliderLocal == null) return;
                colliderLocal.radius = Range;
                SetupCircle();
            }
        }

        public float MinDamage;
        public float MaxDamage;
        public float FiringSpeed;
        public float Range;

        public GameObject[] Priorities;
        public GameObject[] Filters;
        public GameObject BulletPrefab;
        public Transform RotationPoint;
        public Transform ShootingPoint;

        public List<TowerUpgradeLineNode> Upgrades = new List<TowerUpgradeLineNode>();

        private LineRenderer _circleRenderer;
        private float _cooldown;
        private float _cooldownLeft;

        private readonly List<TowerUpgradeLineNode> _appliedUpgrades = new List<TowerUpgradeLineNode>();
        private readonly List<TowerUpgradeLineNode> _upgradesLeft = new List<TowerUpgradeLineNode>();

        private List<EnemyInstance> _enemiesInRange;

        private static GameObject _bulletsParent;

        private static GameObject BulletsParent
        {
            get
            {
                if (_bulletsParent == null)
                    _bulletsParent = GameObject.Find("Bullets");
                return _bulletsParent;
            }
        }

        // Use this for initialization
        private void Start()
        {
            _circleRenderer = GetComponent<LineRenderer>();

            ActualRange = Range;

            _cooldown = 1 / FiringSpeed;
            _cooldownLeft = 0;

            _enemiesInRange = new List<EnemyInstance>();
            _upgradesLeft.AddRange(Upgrades);
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
                var inst = filter.GetComponent<BaseFilter>();
                if (inst != null)
                {
                    targets = inst.FilterEnemies(targets);
                }
            }

            //var target = targets
            //    .OrderBy(e => e.DistanceToGoal.Key).ThenBy(e => e.DistanceToGoal.Value)
            //    .FirstOrDefault();

            foreach (var priority in Priorities)
            {
                targets = priority.GetComponent<BasePriority>().Prioritize(targets);
            }

            var target = targets.FirstOrDefault();

            if (target == null)
            {
                _cooldownLeft = 0;
                return;
            }

            RotateTurret(target.transform);
            Fire(target);
        }

        private void RotateTurret(Transform target)
        {
            if (RotationPoint == null) return; // do not rotate

            var vectorToTarget = target.position - transform.position;
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
            bullet.Damage = Random.Range(MinDamage, MaxDamage);
            //bullet.SpecialEffect = SpecialEffect; TODO: change it
            _cooldownLeft = _cooldown + _cooldownLeft;
        }

        public void Upgrade(TowerUpgradeLineNode upgrade)
        {
            if (!_upgradesLeft.Remove(upgrade))
            {
                Debug.LogError("Invalid upgrade");
                return;
            }

            MinDamage = upgrade.MinAtkIncrease.Apply(MinDamage);
            MaxDamage = upgrade.MaxAtkIncrease.Apply(MaxDamage);
            ActualRange = upgrade.RangeIncrease.Apply(ActualRange);
            FiringSpeed = upgrade.FiringSpeedIncrease.Apply(FiringSpeed);
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

        public List<TowerUpgradeLineNode> GetPossibleUpgrades()
        {
            var result = new List<TowerUpgradeLineNode>();

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
            int vertices = 100;
            float radius = ActualRange;
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
            _circleRenderer.enabled = true;
        }

        public void HideRangeCircle()
        {
            _circleRenderer.enabled = false;
        }
    }
}
