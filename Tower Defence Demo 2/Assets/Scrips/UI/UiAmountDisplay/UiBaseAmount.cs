using UnityEngine;

namespace Scrips.UI.UiAmountDisplay
{
    public abstract class UiBaseAmount : MonoBehaviour
    {
        public abstract void UpdateValue(int amount);
    }
}