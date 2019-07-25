using System;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Variables
{
    [CreateAssetMenu(menuName = "Tower defense kit/Collections/Enemies collection")]
    public class EnemyCollection : RuntimeCollection<EnemyInstance>
    {
    }
}