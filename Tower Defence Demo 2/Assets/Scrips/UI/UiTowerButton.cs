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
        public UiTowerPreBuildInfo UiInfo;

        public void SelectTower()
        {
            SelectedTowerOption.Option.SelectedTowerPrefab = this;
            SelectedTowerImage.sprite = Tower.PreviewSprite;
            SelectedTowerImage.color = Color.white;
        }

        private void Start()
        {
            ButtonImage.sprite = Tower.PreviewSprite;
        }

        public void ShowInfo()
        {
            var rectTransform = UiInfo.GetComponent<RectTransform>();
            rectTransform.position = new Vector3(rectTransform.position.x, transform.position.y + rectTransform.rect.height - 10);
            UiInfo.gameObject.SetActive(true);
        }

        public void HideInfo()
        {
            UiInfo.gameObject.SetActive(false);
        }

        public void BuildTower(TdTile tile)
        {
            tile.Build(Tower);
        }
    }
}