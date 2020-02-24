using Scrips.Instances;
using UnityEngine;

namespace Scrips.Towers.Bullets
{
    public abstract class BulletManager : ScriptableObject
    {
        public abstract BulletInstance SpawnBullet(
            GameObject BulletPrefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent);

        public abstract void DespawnBullet(GameObject bulletInstance);
    }
}