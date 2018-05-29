using UnityEngine;

namespace Scrips.CustomTypes.IncreaseType
{
    [CreateAssetMenu(menuName = "Increase types/Fixed", order = 20)]
    public class FixedIncreaseType : BaseIncreaseType
    {
        public override int Increase(int fromValue, float byValue) => (int)byValue;

        public override double Increase(float fromValue, float byValue) => byValue;
    }
}