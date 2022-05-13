using System.Collections.Generic;
using Scrips.Waves;
using UnityEngine;

namespace Scrips.LevelData.ExtraSteps
{
    [RequireComponent(typeof(WaveGenerator))]
    public class SpawnpointFinder : PostImportComponent
    {
        protected override void PerformPostImport()
        {
            Debug.Log("Finding SpawnPoints for WaveGenerator");
            
            var generator = GetComponent<WaveGenerator>();

            var spawnPoints = GameObject.Find("SpawnPoints");

            if (spawnPoints == null)
            {
                Debug.LogError("SpawnPoints game object not found");
            }

            generator.SpawnpointsToUse = new List<Transform>();

            foreach (Transform concretePoint in spawnPoints.transform)
            {
                generator.SpawnpointsToUse.Add(concretePoint);
            }
        }

        protected override MonoBehaviour GetTargetComponent()
        {
            return GetComponent<WaveGenerator>();
        }
    }
}