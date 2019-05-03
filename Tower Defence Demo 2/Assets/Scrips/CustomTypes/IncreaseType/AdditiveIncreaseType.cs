using UnityEngine;

namespace Scrips.CustomTypes.IncreaseType
{
    [CreateAssetMenu(menuName = "Tower defense kit/Increase types/Additive", order = 0)]
    public class AdditiveIncreaseType : BaseIncreaseType
    {
        public override int Increase(int fromValue, float byValue) => fromValue + (int) byValue;

        public override float Increase(float fromValue, float byValue) => fromValue + byValue;
    }
}