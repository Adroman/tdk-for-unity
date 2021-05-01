using Scrips.Modifiers;
using Scrips.Modifiers.Stats;
using Scrips.Spells;
using Scrips.UI.UiAmountDisplay;
using Scrips.UI.UiProgressDisplay;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public class UiSpellButton : MonoBehaviour
    {
        private class CurrentSpellData
        {
            private readonly FloatModifiableStat _currentCharge;
            private int _currentUsableCharges;
            private readonly UiBaseProgress _progressDisplay;
            private readonly UiBaseAmount _amountDisplay;

            public CurrentSpellData(UiBaseProgress progressDisplay, UiBaseAmount amountDisplay)
            {
                _progressDisplay = progressDisplay;
                _amountDisplay = amountDisplay;
                _currentCharge = new FloatModifiableStat();
            }

            public float CurrentCharge
            {
                get { return _currentCharge.Value; }
                set
                {
                    _currentCharge.Value = value;
                    _progressDisplay.UpdateValue(value);
                }
            }

            public int CurrentUsableCharges
            {
                get { return _currentUsableCharges; }
                set
                {
                    _currentUsableCharges = value;
                    _amountDisplay.UpdateValue(value);
                }
            }

            public float ActualChargeTime { get; set; }
            public float ActualMaxCharges { get; set; }
        }

        public ModifierController ModifierController;
        public EnemySpell Spell;
        public Image ButtonImage;
        public Image SelectedItemImage;
        public Color SpellColor;
        public UiBaseProgress ProgressDisplay;
        public UiBaseAmount AmountDisplay;

        private CurrentSpellData _spellData;

        private FloatModifiableStat _spellRange;
        private FloatModifiableStat _spellInitialCharge;
        private FloatModifiableStat _spellChargeTime;
        private FloatModifiableStat _spellCharges;

        private void Start()
        {
            ButtonImage.sprite = Spell.PreviewSprite;

            _spellRange = new FloatModifiableStat{Value = Spell.Range};
            _spellInitialCharge = new FloatModifiableStat{Value = Spell.InitialCharge};
            _spellChargeTime = new FloatModifiableStat{Value = Spell.ChargeTime};
            _spellCharges = new FloatModifiableStat{Value = Spell.Charges};

            ModifierController.ImportModifiers(this);

            _spellData = new CurrentSpellData(ProgressDisplay, AmountDisplay)
            {
                CurrentCharge = _spellInitialCharge.Value,
                CurrentUsableCharges = Mathf.FloorToInt(_spellInitialCharge.Value),
                ActualChargeTime = _spellChargeTime.Value,
                ActualMaxCharges = _spellCharges.Value
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
        }

        public void TryInstantiateSpell(SpellSpawner spawner)
        {
            if (_spellData.CurrentUsableCharges > 0)
            {
                _spellData.CurrentUsableCharges--;
                Instantiate(Spell.Prefab, spawner.transform.position, spawner.transform.rotation);
            }
        }
    }
}