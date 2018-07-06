using Scrips.Instances;
using Scrips.Towers.BaseData;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips
{
    public class TowerOption : MonoBehaviour
    {
        public int Price;
        public GameObject TowerToSelect;
        public GameObject TowerPrefab;
        public SelectedTowerOption CurrentlySelected;

        private bool _readyToSelect;

        // Use this for initialization
        void Start ()
        {

        }

        // Update is called once per frame
        void Update ()
        {

        }

        public void OnMouseDown()
        {
            _readyToSelect = true;
        }

        public void OnMouseUp()
        {
            if (_readyToSelect)
            {
                _readyToSelect = false;
                CurrentlySelected.SelectedTowerPrefab = TowerPrefab;
                CurrentlySelected.Price = Price;

                GameObject.Find("SelectedTowerImage").GetComponent<Image>().sprite =
                    TowerPrefab.GetComponent<TowerInstance>().PreviewSprite;
            }
        }

        private void OnMouseExit()
        {
            _readyToSelect = false;
        }
    }
}
