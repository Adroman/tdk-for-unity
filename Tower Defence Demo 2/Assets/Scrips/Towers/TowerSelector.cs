using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Towers
{
    public class TowerSelector : MonoBehaviour
    {
        public TowerUiData SelectedTower { get; private set; }

        public void SelectTower(TowerUiData selectedTower)
        {
            SelectedTower = selectedTower;
        }
    }
}