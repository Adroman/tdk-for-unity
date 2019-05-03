using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Spells
{
    [CreateAssetMenu(menuName = "Tower defense kit/Spells/Drain Spell")]
    public class DrainSpell : EnemySpell
    {
        [Range(0, 1)]
        public float PercentageAmount;

        public override void ApplySpell(EnemyInstance enemy)
        {
            enemy.TakeDamage(enemy.Hitpoints * PercentageAmount, true);
        }
    }
}