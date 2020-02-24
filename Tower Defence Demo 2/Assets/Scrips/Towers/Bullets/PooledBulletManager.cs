using Scrips.Pooling;
using UnityEngine;

namespace Scrips.Towers.Bullets
{
    [CreateAssetMenu(menuName = "Tower defense kit/Bullet manager/Pooled")]
    public class PooledBulletManager : BulletManager
    {
        public override BulletInstance SpawnBullet(GameObject BulletPrefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var poolComponent = BulletPrefab.GetComponent<PoolComponent>();

            var bullet = Pooling.PoolManager.Spawn(poolComponent, position, rotation, parent).GetComponent<BulletInstance>();
            bullet.BulletManager = this;
            return bullet;
        }

        public override void DespawnBullet(GameObject bulletInstance)
        {
            Pooling.PoolManager.Despawn(bulletInstance.GetComponent<PoolComponent>());
        }
    }
}