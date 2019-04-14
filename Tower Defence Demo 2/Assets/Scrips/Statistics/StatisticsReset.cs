using System.Collections.Generic;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Statistics
{
    public class StatisticsReset : MonoBehaviour
    {
        public List<IntVariable> IntVariablesToReset;
        public List<FloatVariable> FloatVariablesToReset;
        public List<BooleanVariable> BoolVariablesToReset;

        private void Start()
        {
            foreach (var intVariable in IntVariablesToReset)
            {
                intVariable.Value = 0;
            }

            foreach (var floatVariable in FloatVariablesToReset)
            {
                floatVariable.Value = 0;
            }

            foreach (var booleanVariable in BoolVariablesToReset)
            {
                booleanVariable.Value = false;
            }
        }
    }
}