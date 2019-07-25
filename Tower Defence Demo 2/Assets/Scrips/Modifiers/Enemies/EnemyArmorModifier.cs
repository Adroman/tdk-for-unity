using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Modifiers.Enemies
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Enemy/Armor modifier")]
    public class EnemyArmorModifier : BaseEnemyModifer
    {
        public override void AddToEnemy(EnemyInstance enemy)
        {
            enemy.InitialArmor.AddModifier(this);
        }

        public override void RemoveFromEnemy(EnemyInstance enemy)
        {
            enemy.InitialArmor.RemoveModifier(this);
        }
    }
}