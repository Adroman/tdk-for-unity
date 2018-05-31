using System;
using JetBrains.Annotations;

namespace Scrips.Variables
{
    [PublicAPI]
    [Serializable]
    public class IntReference : Reference<int, IntVariable>
    {
    }
}