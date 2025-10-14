using System.Collections;
using System.Collections.Generic;
using Game.Source.Tags;
using UnityEngine;

namespace Game.Source.Data
{
    public class GunpowderItem : BaseItem
    {
        public GunpowderItem()
        {
            Get<TagPfb>().Prefab = "Prefab/Items/Gunpowder".Load<InteractiveObject>();
            
            var baseDuration = Define<TagBaseUseDuration>();
            var level = Get<TagItemLevel>();

            baseDuration.Duration = new List<float>(level.MaxLevel + 1)
            {
                2f, 1.9f, 1.8f, 1.7f, 1.6f, 1.5f 
            };
            
            var duration = Define<TagUseDuration>().Duration = baseDuration.Get(level); 

            Define<TagIncreaseDamageMultiplier>().Delta = new List<float>(level.MaxLevel + 1)
            {
                0.25f, 0.50f, 0.75f, 1f, 1.25f, 1.5f
            };
            
            var palette = Get<TagColorPaletteProvider>();
            palette.BaseColor = Color.gray1;
            palette.BaseColor = Color.darkRed;
            
            Get<TagName>().Name = "Gunpowder";
            Get<TagDescription>().Loc = "On use: increase damage multiplier by " + Get<TagIncreaseDamageMultiplier>().Delta.ToString();
        }
    }
    public class TagIncreaseDamageMultiplier : ModifiableComponentDefinition
    {
        public List<float> Delta;

        public float Get(TagItemLevel level)
        {
            return Delta[level.Level];
        }
    }

    public class DamageMultiplierIncreaseInteraction : BaseInteraction, IOnUse
    {
        public IEnumerator OnUse(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea)
        {
            if (itemState.Model.Is<TagIncreaseDamageMultiplier>(out var mtInc))
            {
                var duration = itemState.Get<TagUseDuration>();
                var level = itemState.Get<TagItemLevel>();
                yield return new WaitUntil(G.Ticker.CreatePr(duration.Duration));
                yield return sideTurns.IncreaseDamageMultiplier(mtInc.Get(level));
            }
        }
    }
}