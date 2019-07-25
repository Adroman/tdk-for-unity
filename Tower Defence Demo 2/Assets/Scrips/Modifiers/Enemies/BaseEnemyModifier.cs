using Scrips.EnemyData.Instances;

namespace Scrips.Modifiers.Enemies
{
    public abstract class BaseEnemyModifer : BaseModifier
    {
        public abstract void AddToEnemy(EnemyInstance enemy);

        public abstract void RemoveFromEnemy(EnemyInstance enemy);
    }
 }