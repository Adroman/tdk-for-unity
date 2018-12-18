using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips
{
    public class SelectedTowerOption : MonoBehaviour
    {
        [HideInInspector]
        public int Price;

        public static SelectedTowerOption Option { get; private set; }

        public TowerData SelectedTowerPrefab;

        private void Awake()
        {
            if (Option != null)
                Debug.LogError("More than one selection display");
            Option = this;
        }
    }
}
