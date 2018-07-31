using System.Collections;
using Scrips.Towers.BaseData;
using Scrips.Towers.Specials;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiTowerPreBuildInfo : MonoBehaviour
    {
        public TowerData Tower;

        public Text Name;
        public Text Damage;
        public Text FiringSpeed;
        public Text Range;
        public UiSpecials Specials;
        public UiIntTextWithImage Price;

        private void Start()
        {
            Name.text = Tower.TowerName;
            Damage.text = $"Damage: {Tower.MinDamage} - {Tower.MaxDamage}";
            FiringSpeed.text = $"Firing speed: {Tower.FiringSpeed}";
            Range.text = $"Range: {Tower.Range}";

            foreach (var special in Tower.Specials)
            {
                var uiSpecial = Instantiate(Specials.PrefabsToGenerate, Specials.transform);
                StartCoroutine(UpdateTextAfterFrame(uiSpecial, special));
            }

            Price.InitValue(Tower.Price[0]);
        }

        private IEnumerator UpdateTextAfterFrame(UiSpecialText uiSpecial, SpecialType special)
        {
            yield return null;
            uiSpecial.UpdateText(special);
        }
    }
}