using UnityEngine;

namespace Scrips.Towers.Bullets
{
    [CreateAssetMenu(menuName = "Tower defense kit/Bullet manager/Simple")]
    public class SimpleBulletManager : BulletManager
    {
        public override BulletInstance SpawnBullet(GameObject BulletPrefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var bullet = Instantiate(BulletPrefab, position, rotation, parent).GetComponent<BulletInstance>();
            bullet.BulletManager = this;
            return bullet;
        }

        public override void DespawnBullet(GameObject bulletInstance)
        {
            Destroy(bulletInstance);
        }
    }
}