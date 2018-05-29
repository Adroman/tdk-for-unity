using Scrips.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.Instances
{
	public class TextInstance : MonoBehaviour
	{
		public string Prefix;

		private Text _textField;

		public IntReference ValueData;


		// Use this for initialization
		void Awake ()
		{
			_textField = GetComponent<Text>();
		}

		public void UpdateTextValue()
		{
			_textField.text = Prefix + ValueData.Value;
		}
	}
}
