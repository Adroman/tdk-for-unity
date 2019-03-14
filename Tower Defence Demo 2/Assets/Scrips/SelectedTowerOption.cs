using Scrips.Spells;
using Scrips.Towers.BaseData;
using Scrips.UI;
using UnityEngine;

namespace Scrips
{
    public class SelectedTowerOption : MonoBehaviour
    {
        [HideInInspector]
        public int Price;

        public static SelectedTowerOption Option { get; private set; }

        [SerializeField]
        private UiTowerButton _selectedTower;

        [SerializeField]
        private UiSpellButton _selectedSpell;

        public UiTowerButton SelectedTowerPrefab
        {
            get { return _selectedTower; }
            set
            {
                _selectedTower = value;
                _selectedSpell = null;
            }
        }

        public UiSpellButton SelectedSpell
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
