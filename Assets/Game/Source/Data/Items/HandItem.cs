using System.Collections;
using System.Collections.Generic;
using Game.Source.Tags;
using UnityEngine;

namespace Game.Source.Data
{
    public class HandItem : BaseItem
    {
        public HandItem()
        {
            Get<TagPfb>().Prefab = "Prefab/Items/Hand".Load<InteractiveObject>();
            
            var baseDuration = Define<TagBaseUseDuration>();
            var level = Get<TagItemLevel>();

            baseDuration.Duration = new List<float>(level.MaxLevel + 1)
            {
                0.5f, 0.4f, 0.3f, 0.2f, 0.1f, 0f
            };
            
            var duration = Define<TagUseDuration>().Duration = baseDuration.Get(level); 

            Define<TagDecreaseUseTime>();

            var palette = Get<TagColorPaletteProvider>();
            palette.BaseColor = Color.coral;
            palette.BaseColor = Color.aquamarine;
            
            
            Get<TagName>().Name = "Hand";
            Get<TagDescription>().Loc = "On use: set use time of the next item to 0";
        }
    }
    
    public class TagDecreaseUseTime : EntityComponentDefinition
    {
    }
    public class NegateCooldownOfTheNext : BaseInteraction, IOnUse
    {
        public IEnumerator OnUse(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea)
        {
            if (itemState.Model.Is<TagDecreaseUseTime>(out var decrease))
            {
                for (int i = sideTurns.CurrentItemIndex + 1; i < slotArea.InvSlots.Count; i++)
                {
                    if (slotArea.InvSlots[i].InteractiveObject != null)
                    {
                        InteractiveObject itemToBuff = slotArea.InvSlots[i].InteractiveObject;
                        
                        var duration = itemState.Get<TagUseDuration>();
                        
                        itemState.View.MoveToAndBack(itemToBuff.Moveable.TargetPosition, duration.Duration);
                        yield return new WaitUntil(G.Ticker.CreatePr(duration.Duration));
                        
                        itemToBuff.Spin();
                        
                        yield return G.main.NegateCooldown(itemToBuff);

                        itemToBuff.UpdateTimeText();
                        break;
                    }
                }
            }
        }
    }
}