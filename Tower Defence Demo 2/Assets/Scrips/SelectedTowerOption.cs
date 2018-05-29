using UnityEngine;

namespace Scrips
{
    public class SelectedTowerOption : MonoBehaviour
    {
        private GameObject _selectedTowerPrefab;

        [HideInInspector]
        public int Price;

        public static SelectedTowerOption Option { get; private set; }

        public GameObject SelectedTowerPrefab
        {
            get { return _selectedTowerPrefab; }
            set
            {
                _selectedTowerPrefab = value;
                //GetComponent<SpriteRenderer>().sprite = value.GetComponent<SpriteRenderer>().sprite;
            }
        }

        private void Awake()
        {
            if (Option != null)
                Debug.LogError("More than one selection dislplay");
            Option = this;
        }
    }
}
