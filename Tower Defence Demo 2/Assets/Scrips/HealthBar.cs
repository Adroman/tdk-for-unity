using UnityEngine;

namespace Scrips
{
	public class HealthBar : MonoBehaviour {

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public void UpdateHealthBar(float actualHealth, float maxHealth)
		{
			transform.localScale = new Vector3(0.5f, 5 * (actualHealth / maxHealth), 1f);
		}
	}
}
