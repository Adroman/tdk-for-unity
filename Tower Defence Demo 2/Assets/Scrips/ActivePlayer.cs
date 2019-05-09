using System;
using UnityEngine;

namespace Scrips
{
    public class ActivePlayer : MonoBehaviour
    {
        public PlayerData Active;

        private void Start()
        {
            PlayerData.ActivePlayer = Active;
        }
    }
}