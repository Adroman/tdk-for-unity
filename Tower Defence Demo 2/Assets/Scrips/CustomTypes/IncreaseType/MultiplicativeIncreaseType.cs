using UnityEngine;

namespace Scrips.CustomTypes.IncreaseType
{
    [CreateAssetMenu(menuName = "Increase types/Multiplicative", order = 10)]
    public class MultiplicativeIncreaseType : BaseIncreaseType
    {
        public override int Increase(int fromValue, float byValue) => (int) (fromValue * byValue);

        public override double Increase(float fromValue, float byValue) => fromValue * byValue;
    }
}