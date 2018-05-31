using JetBrains.Annotations;
using UnityEngine;

namespace Scrips.Variables
{
    [PublicAPI]
    [CreateAssetMenu(menuName = "Variables/Int variable")]
    public class IntVariable : Variable<int>
    {
    }
}