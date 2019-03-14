using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Spells
{
    public abstract class EnemySpell : ScriptableObject
    {
        public float Range;

        public float Charges;

        public float ChargeTime;

        public float InitialCharge;

        public Sprite PreviewSprite;

        public SpellInstance Prefab;

        public abstract void ApplySpell(EnemyInstance enemy);
    }
}