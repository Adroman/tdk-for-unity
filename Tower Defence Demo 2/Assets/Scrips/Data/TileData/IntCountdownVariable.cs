using System.Collections;
using JetBrains.Annotations;
using Scrips.Variables;
using UnityEngine;

namespace Data
{
    [PublicAPI]
    public class IntCountdown : MonoBehaviour
    {
        public IntVariable VariableToUse;

        public void ResetCountDown(int value)
        {
            StopAllCoroutines();
            VariableToUse.Value = value;
        }

        public void StartCountDown(int initialValue)
        {
            ResetCountDown(initialValue);
            StartCoroutine(CountDown());
        }

        public void StopCountdown()
        {
            StopAllCoroutines();
        }

        private IEnumerator CountDown()
        {
            while (VariableToUse.Value > 0)
            {
                yield return new WaitForSeconds(1);
                VariableToUse.Value--;
            }
        }
    }
}