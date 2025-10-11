using System.Collections;
using System.Collections.Generic;
using Game.Source.Tags;
using UnityEngine;

namespace Game.Source.Data
{
    public class ImprovedSkullItem : BaseItem
    {
        public ImprovedSkullItem()
        {
            var useDuration = Get<TagUseDuration>();
            useDuration.BaseDuration = 0.5f;
            useDuration.Duration = 0.5f;

            Define<TagImproveDefMul>().DefMultDelta = 0.1f;
            
            Get<TagName>().Name = "Skull";
            Get<TagDescription>().Loc = "Another item used: increase defense mult by " + Get<TagImproveDefMul>().DefMultDelta;
        }
    }
    public class TagImproveDefMul : ModifiableComponentDefinition
    {
        public float DefMultDelta;
    }

    public class IncreaseMultOnNotUsed : BaseInteraction, IOnNotUsed
    {
        public IEnumerator OnNotUsed(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea)
        {
            if (itemState.Is<TagImproveDefMul>(out var defImpr))
            {
                yield return sideTurns.IncreaseDefMultiplier(defImpr.DefMultDelta);
            }
        }
    }
    public interface IOnNotUsed
    {
        public IEnumerator OnNotUsed(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea);
    }
}