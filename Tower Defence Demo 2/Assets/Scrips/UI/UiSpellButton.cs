using Scrips.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiSpellButton : MonoBehaviour
    {
        private class CurrentSpellData
        {
            private float _currentCharge;
            private int _currentUsableCharges;
            private readonly UiSpellButton _spellButton;

            public CurrentSpellData(UiSpellButton button)
            {
                _spellButton = button;
            }

            public float CurrentCharge
            {
                get { return _currentCharge; }
                set
                {
                    _currentCharge = value;
                    _spellButton.SpellChargeMeter.fillAmount = 1 - _currentCharge;
                }
            }

            public int CurrentUsableCharges
            {
                get { return _currentUsableCharges; }
                set
                {
                    _currentUsableCharges = value;
                    _spellButton.SpellChargeNumber.text = _currentUsableCharges.ToString();
                }
            }

            public float ActualChargeTime { get; set; }
            public float ActualMaxCharges { get; set; }
        }

        public EnemySpell Spell;
        public Image ButtonImage;
        public Image SelectedItemImage;
        public Color SpellColor;
        public Image SpellChargeMeter;
        public Text SpellChargeNumber;

        private CurrentSpellData _spellData;

        private void Start()
        {
            ButtonImage.sprite = Spell.PreviewSprite;

            _spellData = new CurrentSpellData(this)
            {
                CurrentCharge = Spell.InitialCharge,
                CurrentUsableCharges = Mathf.FloorToInt(Spell.InitialCharge),
                ActualChargeTime = Spell.ChargeTime,
                ActualMaxCharges = Spell.Charges
            };
        }

        public void SelectSpell()
        {
            SelectedTowerOption.Option.SelectedSpell = this;
            SelectedItemImage.sprite = Spell.PreviewSprite;
            SelectedItemImage.color = SpellColor;
        }

        public void Update()
        {
            if (_spellData.CurrentCharge + _spellData.CurrentUsableCharges >= _spellData.ActualMaxCharges)
            {
                _spellData.CurrentCharge = _spellData.ActualMaxCharges - _spellData.CurrentUsableCharges;
                return;
            }

            _spellData.CurrentCharge += (1f / _spellData.ActualChargeTime) * Time.deltaTime;

            if (_spellData.CurrentCharge >= 1)
            {
                _spellData.CurrentUsableCharges++;
                _spellData.CurrentCharge -= 1f;
            }

            AdjustGraphics();
        }

        private void AdjustGraphics()
        {
            SpellChargeMeter.fillAmount = 1 - _spellData.CurrentCharge;
            SpellChargeNumber.text = _spellData.CurrentUsableCharges.ToString();
        }

        public void TryInstantiateSpell(SpellSpawner spawner)
        {
            if (_spellData.CurrentUsableCharges > 0)
            {
                _spellData.CurrentUsableCharges--;
                Instantiate(Spell.Prefab, spawner.transform.position, spawner.transform.rotation);
                AdjustGraphics();
            }
        }
    }
}