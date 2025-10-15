using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Source.Data.Characters;
using Game.Source.Utils;
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

    public class DamageIncreaseDescription : IDescriptionProvider
    {
        public string GetDescription(ItemState state)
        {
            if (state.Model.Is<TagIncreaseDamage>(out var tag))
            {
                var level = state.Get<TagItemLevel>();
                var value = tag.Get(level);
                string description = $"On use: increases <color=red> gun damage </color> by <color=green> {value}</color>";
                return description;
            }
            return "";
        }
    }



    public interface IOnUse
    {
        public IEnumerator OnUse(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea);
    }
}