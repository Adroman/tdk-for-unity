using UnityEngine;

namespace Scrips.UI
{
    public class UiWave : MonoBehaviour
    {
        public void Despawn()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            Destroy(gameObject);
        }
    }
}