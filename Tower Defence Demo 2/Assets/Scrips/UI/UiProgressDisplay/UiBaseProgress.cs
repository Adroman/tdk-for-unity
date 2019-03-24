using UnityEngine;

namespace Scrips.UI.UiProgressDisplay
{
    public abstract class UiBaseProgress : MonoBehaviour
    {
        public abstract void UpdateValue(float amount);
    }
}