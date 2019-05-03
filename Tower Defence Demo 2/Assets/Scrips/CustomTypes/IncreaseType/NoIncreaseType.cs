using UnityEngine;

namespace Scrips.CustomTypes.IncreaseType
{
    [CreateAssetMenu(menuName = "Tower defense kit/Increase types/None", order = 30)]
    public class NoIncreaseType : BaseIncreaseType
    {
        public override int Increase(int fromValue, float byValue) => fromValue;

        public override float Increase(float fromValue, float byValue) => fromValue;
    }
}