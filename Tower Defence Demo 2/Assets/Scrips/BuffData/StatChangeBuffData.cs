using System.Linq;
using Scrips.CustomTypes.IncreaseType;
using Scrips.EnemyData.Instances;
using Scrips.Extensions;
using UnityEngine;

namespace Scrips.BuffData
{
    public abstract class StatChangeBuffData : BaseBuffData
    {
        public float Amount { get; }

        public BaseIncreaseType AmountType { get; }

        protected abstract float StatToChange { get; set; }

        public override float Power { get; }

        public StatChangeBuffData(EnemyInstance target, float amount, BaseIncreaseType amountType, float duration) : base(target, duration)
        {
            Amount = amount;
            AmountType = amountType;
            Power =  1f / AmountType.Increase(StatToChange, Amount);
        }

        protected override void ActivateEffect()
        {
            CalculateCurrentStat();
        }

        protected override void FinishEffect()
        {
            CalculateCurrentStat();
        }

        protected override void UpdateEffect(float deltaTime)
        {
            // nothing
        }

        private void CalculateCurrentStat()
        {
            float minimumStat = StatToChange;
            foreach (var debuff in from b in Target.ActiveDebuffs where b.GetType() == GetType() select (StatChangeBuffData)b)
            {
                float speedFromThisDebuff = AmountType.Increase(StatToChange, debuff.Amount);
                minimumStat = Mathf.Min(minimumStat, speedFromThisDebuff);
            }

            StatToChange = minimumStat;
        }
    }
}