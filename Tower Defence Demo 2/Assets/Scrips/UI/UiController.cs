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

        private bool _pausingEnabled;

        public void Pause()
        {
            if (!_pausingEnabled) return;
            Time.timeScale = 0;
            InGameUi.gameObject.SetActive(false);
            PauseUi.gameObject.SetActive(true);
            _paused = true;
        }

        public void Unpause()
        {
            if (!_pausingEnabled) return;
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
            _pausingEnabled = true;
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

        public void SwitchToVictoryUi()
        {
            _pausingEnabled = false;
            Time.timeScale = 0;
            InGameUi.gameObject.SetActive(false);
            PauseUi.gameObject.SetActive(false);
            DefeatUi.gameObject.SetActive(false);
            VictoryUi.gameObject.SetActive(true);
        }

        public void SwitchToDefeatUi()
        {
            _pausingEnabled = false;
            Time.timeScale = 0;
            InGameUi.gameObject.SetActive(false);
            PauseUi.gameObject.SetActive(false);
            VictoryUi.gameObject.SetActive(false);
            DefeatUi.gameObject.SetActive(true);
        }
    }
}