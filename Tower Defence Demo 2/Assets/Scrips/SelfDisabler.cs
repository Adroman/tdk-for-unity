using UnityEngine;

namespace Scrips
{
    public class SelfDisabler : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}