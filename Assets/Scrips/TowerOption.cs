//using Scrips.Instances;
//using Scrips.Towers.BaseData;
//using UnityEngine;
//using UnityEngine.UI;
//
//namespace Scrips
//{
//    public class TowerOption : MonoBehaviour
//    {
//        public int Price;
//        public TowerData TowerPrefab;
//        public SelectedTowerOption CurrentlySelected;
//
//        private bool _readyToSelect;
//
//        public void OnMouseDown()
//        {
//            _readyToSelect = true;
//        }
//
//        public void OnMouseUp()
//        {
//            if (_readyToSelect)
//            {
//                _readyToSelect = false;
//                CurrentlySelected.SelectedTowerPrefab = TowerPrefab;
//                CurrentlySelected.Price = Price;
//
//                GameObject.Find("SelectedTowerImage").GetComponent<Image>().sprite =
//                    TowerPrefab.PreviewSprite;
//            }
//        }
//
//        private void OnMouseExit()
//        {
//            _readyToSelect = false;
//        }
//    }
//}
