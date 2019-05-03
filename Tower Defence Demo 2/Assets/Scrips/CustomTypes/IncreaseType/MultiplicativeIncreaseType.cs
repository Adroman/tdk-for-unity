using UnityEngine;

namespace Scrips.CustomTypes.IncreaseType
{
    [CreateAssetMenu(menuName = "Tower defense kit/Increase types/Multiplicative", order = 10)]
    public class MultiplicativeIncreaseType : BaseIncreaseType
    {
        public override int Increase(int fromValue, float byValue) => (int) (fromValue * byValue);

        public override float Increase(float fromValue, float byValue) => fromValue * byValue;
    }
}