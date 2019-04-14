using System.Collections.Generic;
using Scrips.Objectives.Rewards;
using UnityEngine;

namespace Scrips.Objectives
{
    // Inspired from this site: https://forum.unity.com/threads/goals-missions-objective-system.246842/

    public abstract class BaseObjective : MonoBehaviour
    {
        protected abstract bool IsCompleted();
        public List<BaseReward> Rewards;

        [SerializeField]
        private bool _isCompleted = false;

        public void CheckObjective()
        {
            if (!_isCompleted && IsCompleted())
            {
                _isCompleted = true;
                foreach (var reward in Rewards)
                {
                    reward.RedeemReward();
                }
            }
        }
    }
}