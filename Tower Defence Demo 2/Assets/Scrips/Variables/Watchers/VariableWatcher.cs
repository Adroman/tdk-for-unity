using UnityEngine;

namespace Scrips.Variables.Watchers
{
    public interface IVariableWatcher<in T>
    {
        void Raise(T value);
    }
}