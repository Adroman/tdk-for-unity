using Scrips.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiSpellButton : MonoBehaviour
    {
        public EnemySpell Spell;
        public Image ButtonImage;
        public Image SelectedItemImage;
        public Color SpellColor;

        private void Start()
        {
            ButtonImage.sprite = Spell.PreviewSprite;
        }

        public void SelectSpell()
        {
            SelectedTowerOption.Option.SelectedSpell = Spell;
            SelectedItemImage.sprite = Spell.PreviewSprite;
            SelectedItemImage.color = SpellColor;
        }
    }
}