using UnityEngine;

namespace Scrips.CustomTypes.IncreaseType
{
    [CreateAssetMenu(menuName = "Increase types/None", order = 30)]
    public class NoIncreaseType : BaseIncreaseType
    {
        public override int Increase(int fromValue, float byValue) => fromValue;

        public override double Increase(float fromValue, float byValue) => fromValue;
    }
}