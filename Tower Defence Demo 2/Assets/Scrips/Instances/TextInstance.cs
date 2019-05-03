using Scrips.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.Instances
{
	public class TextInstance : MonoBehaviour
	{
		public string Prefix;

		private Text _textField;

		private TextMeshProUGUI _tmpTextField;

		public IntReference ValueData;


		// Use this for initialization
		void Awake ()
		{
			_textField = GetComponent<Text>();
			_tmpTextField = GetComponent<TextMeshProUGUI>();
		}

		public void UpdateTextValue()
		{
			if (_tmpTextField != null) _tmpTextField.text = Prefix + ValueData.Value;
			if (_textField != null) _textField.text = Prefix + ValueData.Value;
		}
	}
}
