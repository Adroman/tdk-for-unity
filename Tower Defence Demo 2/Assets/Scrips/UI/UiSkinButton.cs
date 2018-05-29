using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class UiSkinButton : UiSkin
    {
        private Button _button;
        private Image _image;
    }
}