using System.Collections.Generic;
using Scrips.EnemyData.Instances;
using Scrips.Modifiers.Currency;
using UnityEngine;

namespace Scrips.Modifiers.Enemies
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Enemy/Punishment modifier")]
    public class EnemyPunishmentModifier : BaseEnemyCurrencyModifier
    {
        protected override List<ModifiedCurrency> GetDesiredCollection(EnemyInstance enemyInstance)
        {
            return enemyInstance.IntPunishments;
        }
    }
}