using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scrips.UI
{
    [Serializable]
    public class KeyEvent
    {
        public KeyCode KeyCode;
        public UnityEvent Response;
    }

    public class UiController : MonoBehaviour
    {
        public Canvas InGameUi;

        public Canvas PauseUi;

        public Canvas VictoryUi;

        public Canvas DefeatUi;

        public List<KeyEvent> KeyEvents;

        private bool _paused;

        public void Pause()
        {
            Time.timeScale = 0;
            InGameUi.gameObject.SetActive(false);
            PauseUi.gameObject.SetActive(true);
            _paused = true;
        }

        public void Unpause()
        {
            Time.timeScale = 1;
            PauseUi.gameObject.SetActive(false);
            InGameUi.gameObject.SetActive(true);
            _paused = false;
        }

        public void TogglePause()
        {
            if (_paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }

        private void Start()
        {
            _paused = false;
        }

        private void Update()
        {
            foreach (var keyEvent in KeyEvents)
            {
                if (Input.GetKeyUp(keyEvent.KeyCode))
                {
                    keyEvent.Response.Invoke();
                }
            }
        }
    }
}