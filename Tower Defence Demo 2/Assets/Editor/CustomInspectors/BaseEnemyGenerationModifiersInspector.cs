using System.Text;
using Scrips.Waves;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(BaseEnemyGenerationModifiers))]
    public class BaseEnemyGenerationModifiersInspector : UnityEditor.Editor
    {
        private BaseEnemyGenerationModifiers _target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _target = (BaseEnemyGenerationModifiers) target;

            if (GUILayout.Button("Test"))
            {
                var rand = new System.Random(_target.TestRandomSeed.GetHashCode());

                var generatedCluster = _target.GenerateCluster(_target.TestDifficulty, _target.TestRatio, rand);

                var builder = new StringBuilder()
                    .Append("Test Cluster: ").AppendLine()
                    .Append("Amount: ").Append(generatedCluster.Amount).AppendLine()
                    .Append("Interval: ").Append(generatedCluster.Interval).AppendLine()
                    .Append("Interval Deviation: ").Append(generatedCluster.IntervalDeviation).AppendLine()
                    .Append("Initial Countdown: ").Append(generatedCluster.InitialCountDown).AppendLine()
                    .Append("EnemyData: ").AppendLine()
                    .Append("  Initial HP: ").Append(generatedCluster.EnemyData.InitialHitpoints).AppendLine()
                    .Append("  Initial Armor: ").Append(generatedCluster.EnemyData.InitialArmor).AppendLine()
                    .Append("  Initial Speed: ").Append(generatedCluster.EnemyData.InitialSpeed).AppendLine()
                    .Append("  HP Deviation: ").Append(generatedCluster.EnemyData.HitpointsDeviation).AppendLine()
                    .Append("  Armor Deviation: ").Append(generatedCluster.EnemyData.ArmorDeviation).AppendLine()
                    .Append("  Speed Deviation: ").Append(generatedCluster.EnemyData.SpeedDeviation).AppendLine()
                    .Append("  Loot: ").AppendLine();
                foreach (var loot in generatedCluster.EnemyData.IntLoots)
                {
                    builder.Append($"    {loot.Variable.name} {loot.Amount}").AppendLine();
                }

                builder.Append("  Punishments: ").AppendLine();
                foreach (var punishments in generatedCluster.EnemyData.IntPunishments)
                {
                    builder.Append($"    {punishments.Variable.name} {punishments.Amount}").AppendLine();
                }

                Debug.Log(builder.ToString());
            }
        }
    }
}