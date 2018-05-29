using System;
using UnityEngine;

namespace Editor.Utils
{
    public class IsolatedHorizontal : IDisposable
    {
        public IsolatedHorizontal()
        {
            GUILayout.BeginHorizontal();
        }

        public void Dispose()
        {
            GUILayout.EndHorizontal();
        }
    }
}