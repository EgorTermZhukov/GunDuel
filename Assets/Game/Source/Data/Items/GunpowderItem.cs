using System.Collections;
using Game.Source.Tags;
using UnityEngine;

namespace Game.Source.Data
{
    public class GunpowderItem : BaseItem
    {
        public GunpowderItem()
        {
            Get<TagPfb>().Prefab = "Prefab/Items/Gunpowder".Load<InteractiveObject>();
            
            var duration = Get<TagUseDuration>(); 
            duration.BaseDuration = 2f;
            duration.Duration = 2f;
            
            var palette = Get<TagColorPaletteProvider>();
            palette.BaseColor = Color.gray1;
            palette.BaseColor = Color.black;

            Define<TagIncreaseDamageMultiplier>().Delta = 0.25f;
            
            Get<TagName>().Name = "Gunpowder";
            Get<TagDescription>().Loc = "On use: increase damage multiplier by " + Get<TagIncreaseDamageMultiplier>().Delta.ToString();
        }
    }
    public class TagIncreaseDamageMultiplier : ModifiableComponentDefinition
    {
        public float Delta;
    }

    public class DamageMultiplierIncreaseInteraction : BaseInteraction, IOnUse
    {
        public IEnumerator OnUse(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea)
        {
            if (itemState.Is<TagIncreaseDamageMultiplier>(out var mtInc))
            {
                yield return sideTurns.IncreaseDamageMultiplier(mtInc.Delta);
            }
        }
    }
}