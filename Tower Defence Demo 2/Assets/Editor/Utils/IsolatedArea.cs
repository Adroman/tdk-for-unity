using System;
using UnityEngine;

namespace Editor.Utils
{
    public struct IsolatedArea : IDisposable
    {
        public IsolatedArea(Rect r)
        {
            GUILayout.BeginArea(r);
        }

        public void Dispose()
        {
            GUILayout.EndArea();
        }
    }
}