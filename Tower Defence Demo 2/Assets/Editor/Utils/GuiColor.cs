using System;
using UnityEngine;

namespace Editor.Utils
{
    public struct GuiColor : IDisposable
    {
        private readonly Color oldColor;
        public GuiColor(Color newColor)
        {
            oldColor = GUI.color;
            GUI.color = newColor;
        }

        public void Dispose()
        {
            GUI.color = oldColor;
        }
    }
}
