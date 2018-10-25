using UnityEngine;

namespace Scrips
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(UiLevel))]
    public class MapLevelCollider : MonoBehaviour
    {
        public GameObject OffState;
        public GameObject OnState;

        private UiLevel _loader;

        private bool _inState;
        private bool _mouseDown;

        private void OnEnable()
        {
            _loader = GetComponent<UiLevel>();
        }

        private void OnMouseEnter()
        {
            if (OnState != null) OnState.SetActive(true);
            if (OffState != null) OffState.SetActive(false);
            _inState = true;
        }

        private void OnMouseExit()
        {
            if (OnState != null) OnState.SetActive(false);
            if (OffState != null) OffState.SetActive(true);
            _inState = false;
        }

        private void OnMouseDown()
        {
            if (_inState) _mouseDown = true;
        }

        private void OnMouseUp()
        {
            if (_inState && _mouseDown) _loader.LoadLevel();
        }
    }
}