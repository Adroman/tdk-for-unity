using System;
using UnityEngine;

namespace Editor.Utils
{
    public class IsolatedScrollView : IDisposable
    {
        public IsolatedScrollView(Rect scrollRect, ref Vector2 scrollPosition, Rect innerRect)
        {
            scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, innerRect);
        }

        public void Dispose()
        {
            GUI.EndScrollView();
        }
    }
}