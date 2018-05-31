using System;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Variables
{
    [Serializable]
    [CreateAssetMenu(menuName = "Collections/Enemies collection")]
    public class EnemyCollection : RuntimeCollection<EnemyInstance>
    {
    }
}