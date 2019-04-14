using System.Collections.Generic;
using Scrips.Modifiers;
using UnityEngine;

namespace Scrips.Objectives.Rewards
{
    public class ActivateModifiersReward : BaseReward
    {
        public List<BaseModifier> ModifiersToActivate;
        public ModifierController ModifierController;

        public override void RedeemReward()
        {
            foreach (var modifier in ModifiersToActivate)
            {
                ModifierController.AddModifier(modifier);
            }
        }
    }
}