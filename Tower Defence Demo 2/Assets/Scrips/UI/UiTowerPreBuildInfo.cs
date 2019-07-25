using System.Collections;
using System.Linq;
using Scrips.Towers.BaseData;
using Scrips.Towers.Specials;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiTowerPreBuildInfo : MonoBehaviour
    {
        public TowerUiData Tower;

        public Text Name;
        public Text Damage;
        public Text FiringSpeed;
        public Text Range;
        public UiSpecials Specials;
        public UiIntTextWithImage Price;

        private void Start()
        {
            Name.text = Tower.BaseTowerData.TowerName;
            Damage.text = $"Damage: {Tower.MinDamage.Value} - {Tower.MaxDamage.Value}";
            FiringSpeed.text = $"Firing speed: {Tower.FiringSpeed.Value}";
            Range.text = $"Range: {Tower.Range.Value}";

            foreach (var special in Tower.BaseTowerData.Specials)
            {
                var uiSpecial = Instantiate(Specials.PrefabsToGenerate, Specials.transform);
                StartCoroutine(UpdateTextAfterFrame(uiSpecial, special));
            }

            Price.InitValue(Tower.GetModifiedPrice().First());
        }

        private IEnumerator UpdateTextAfterFrame(UiSpecialText uiSpecial, SpecialType special)
        {
            yield return null;
            uiSpecial.UpdateText(special);
        }
    }
}