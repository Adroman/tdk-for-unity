using System.Collections.Generic;
using System.Linq;
using Scrips.EnemyData.Instances;
using Scrips.Priorities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scrips.Instances
{
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class TowerInstance : MonoBehaviour
    {
        public float BaseRange;
        public float BaseMinDamage;
        public float BaseMaxDamage;
        public float BaseFiringSpeed;

        private int _level;

        private float ActualRange
        {
            get { return _actualRange; }
            set
            {
                _actualRange = value;
                var colliderLocal = GetComponent<CircleCollider2D>();
                if (colliderLocal == null) return;
                colliderLocal.radius = _actualRange;
                SetupCircle();
            }
        }

        private float _actualRange;
        private float _actualMinDamage;
        private float _actualMaxDamage;
        private float _actualFiringSpeed;

        private float _cooldown;
        private float _cooldownLeft;

        private LineRenderer _circleRenderer;

        public GameObject[] Priorities;
        public GameObject[] Filters;
        public GameObject SpecialEffect;
        public GameObject BulletPrefab;
        public Transform RotationPoint;
        public Transform ShootingPoint;
        public Sprite PreviewSprite;

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

            _level = 1;
            ActualRange = BaseRange;
            _actualMinDamage = BaseMinDamage;
            _actualMaxDamage = BaseMaxDamage;
            _actualFiringSpeed = BaseFiringSpeed;

            _cooldown = 1 / _actualFiringSpeed;
            _cooldownLeft = 0;

            _enemiesInRange = new List<EnemyInstance>();
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

        private Transform GetShootingPointPosition()
        {
            return ShootingPoint != null ? ShootingPoint : transform;
        }

        private void Fire(EnemyInstance target)
        {
            var shootingPoint = GetShootingPointPosition();

            var bullet = PoolManager.Spawn(BulletPrefab, shootingPoint.position, transform.rotation, BulletsParent.transform).GetComponent<BulletInstance>();
            bullet.Target = target;
            bullet.Damage = Random.Range(_actualMinDamage, _actualMaxDamage);
            bullet.SpecialEffect = SpecialEffect;
            _cooldownLeft = _cooldown + _cooldownLeft;
        }

        private void Upgrade()
        {
            _level++;
            _actualFiringSpeed = BaseFiringSpeed + 0.05f * (_level - 1) * BaseFiringSpeed;
            _actualMinDamage = BaseMinDamage + 0.10f * (_level - 1) * BaseMinDamage;
            _actualMaxDamage = BaseMaxDamage + 0.10f * (_level - 1) * BaseMaxDamage;
            ActualRange = BaseRange + 0.05f * (_level - 1) * BaseRange;

            _cooldown = 1 / _actualFiringSpeed;
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
            int vertices = 40;
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
