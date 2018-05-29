using UnityEngine;

namespace Scrips.UI
{
    public class UiSkin : MonoBehaviour
    {
        public UiSkinData SkinData;

        protected virtual void OnSkinUi()
        {
            // do nothing
        }

        public virtual void Awake()
        {
            OnSkinUi();
        }

        public virtual void Update()
        {
            if (Application.isEditor)
            {
                OnSkinUi();
            }
        }
    }
}