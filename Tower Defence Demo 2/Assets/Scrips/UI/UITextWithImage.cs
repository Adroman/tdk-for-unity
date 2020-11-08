using System;
using Scrips.Variables;
using Scrips.Variables.Watchers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    public abstract class UITextWithImage<TVariable, T> : MonoBehaviour, IVariableWatcher<T> where TVariable : Variable<T>
    {
        public TVariable Variable;

        public Image ImageToUse;

        public Text TextToUse;
        private bool _isUsingText;

        public TextMeshProUGUI TmpTextToUse;
        private bool _isUsingTmpText;

        public string Prefix;

        public string Postfix;

        private void OnEnable()
        {
            Variable.AddWatcher(this);
            _isUsingText = TextToUse != null;
            _isUsingTmpText = TmpTextToUse != null;
        }

        private void OnDisable()
        {
            Variable.RemoveWatcher(this);
        }

        private void Start()
        {
            if (ImageToUse != null)
            {
                ImageToUse.sprite = Variable.Icon;
                ImageToUse.color = Variable.IconColor;
            }

            UpdateText();
        }

        public void UpdateText()
        {
            if (_isUsingText)
                TextToUse.text = $"{Prefix}{Variable.Value}{Postfix}";

            if (_isUsingTmpText)
                TmpTextToUse.text = $"{Prefix}{Variable.Value}{Postfix}";
        }

        public void Raise(T value)
        {
            UpdateText();
        }
    }
}