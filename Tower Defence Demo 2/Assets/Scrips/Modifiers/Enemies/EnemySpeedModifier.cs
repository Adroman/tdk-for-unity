using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Modifiers.Enemies
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Enemy/Speed modifier")]
    public class EnemySpeedModifier : BaseEnemyModifer
    {
        public override void AddToEnemy(EnemyInstance enemy)
        {
            enemy.InitialSpeed.AddModifier(this);
        }

        public override void RemoveFromEnemy(EnemyInstance enemy)
        {
            enemy.InitialSpeed.RemoveModifier(this);
        }
    }
}