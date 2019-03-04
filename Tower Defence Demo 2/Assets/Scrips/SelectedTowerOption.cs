using Scrips.Spells;
using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips
{
    public class SelectedTowerOption : MonoBehaviour
    {
        [HideInInspector]
        public int Price;

        public static SelectedTowerOption Option { get; private set; }

        [SerializeField]
        private TowerData _selectedTower;

        [SerializeField]
        private EnemySpell _selectedSpell;

        public TowerData SelectedTowerPrefab
        {
            get { return _selectedTower; }
            set
            {
                _selectedTower = value;
                _selectedSpell = null;
            }
        }

        public EnemySpell SelectedSpell
        {
            get { return _selectedSpell; }
            set
            {
                _selectedSpell = value;
                _selectedTower = null;
            }
        }

        private void Awake()
        {
            if (Option != null)
                Debug.LogError("More than one selection display");
            Option = this;
        }
    }
}
