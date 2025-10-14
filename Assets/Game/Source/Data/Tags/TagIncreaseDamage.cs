using System;
using System.Collections;
using System.Collections.Generic;
using Game.Source.Data.Characters;
using UnityEngine;

namespace Game.Source.Tags
{
    [Serializable]
    public class TagIncreaseDamage : EntityComponentDefinition
    {
        public List<float> DamageIncrease;

        public float Get(TagItemLevel itemLevel)
        {
            return DamageIncrease[itemLevel.Level];
        }
    }
    public class IncreaseDamageInteraction : BaseInteraction, IOnUse
    {
        public IEnumerator OnUse(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea)
        {
            if (itemState.Model.Is<TagIncreaseDamage>(out var dmg))
            {
                var level = itemState.Get<TagItemLevel>();
                var duration = itemState.Get<TagUseDuration>();
                yield return new WaitUntil(G.Ticker.CreatePr(duration.Duration));
                yield return sideTurns.IncreaseDamage(dmg.Get(level), sideTurns.SideState);
            }
        }
    }
    public interface IOnUse
    {
        public IEnumerator OnUse(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea);
    }
}