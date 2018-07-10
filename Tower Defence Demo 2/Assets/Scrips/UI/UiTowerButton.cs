using Scrips.Towers.BaseData;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiTowerButton : MonoBehaviour
    {
        public TowerData Tower;
        public Image ButtonImage;
        public Image SelectedTowerImage;

        public void SelectTower()
        {
            SelectedTowerOption.Option.SelectedTowerPrefab = Tower;
            SelectedTowerImage.sprite = Tower.PreviewSprite;
            SelectedTowerImage.color = Color.white;
        }

        private void Start()
        {
            ButtonImage.sprite = Tower.PreviewSprite;
        }
    }
}