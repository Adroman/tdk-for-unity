using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Modifiers.Enemies
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Enemy/Hitpoints modifier")]
    public class EnemyHpModifier : BaseEnemyModifer
    {
        public override void AddToEnemy(EnemyInstance enemy)
        {
            enemy.InitialHitpoints.AddModifier(this);
        }

        public override void RemoveFromEnemy(EnemyInstance enemy)
        {
            enemy.InitialHitpoints.RemoveModifier(this);
        }
    }
}