using Scrips.Variables;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Modifiers/Tower/Cost modifier")]
    public class TowerCostModifier : BaseModifier
    {
        public IntVariable Currency;
    }
}