using System.Collections;
using Game.Source.Tags;
using UnityEngine;

namespace Game.Source.Data
{
    public class HandItem : BaseItem
    {
        public HandItem()
        {
            Get<TagPfb>().Prefab = "Prefab/Items/Hand".Load<InteractiveObject>();
            
            var useDuration = Get<TagUseDuration>();
            useDuration.BaseDuration = 0.5f;
            useDuration.Duration = 0.5f;

            var palette = Get<TagColorPaletteProvider>();
            palette.BaseColor = Color.coral;
            palette.BaseColor = Color.aquamarine;
            
            Define<TagDecreaseUseTime>().delta = 0.4f;
            
            Get<TagName>().Name = "Hand";
            Get<TagDescription>().Loc = "On use: set use time of the next item to 0";
        }
    }
    
    public class TagDecreaseUseTime : ModifiableComponentDefinition
    {
        public float delta;
    }

    public class NegateCooldownOfTheNext : BaseInteraction, IOnUse
    {
        public IEnumerator OnUse(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea)
        {
            if (itemState.Is<TagDecreaseUseTime>(out var decrease))
            {
                for (int i = sideTurns.CurrentItemIndex + 1; i < slotArea.FaceSlots.Count; i++)
                {
                    if (slotArea.FaceSlots[i].InteractiveObject != null)
                    {
                        InteractiveObject itemToBuff = slotArea.FaceSlots[i].InteractiveObject;
                        yield return G.main.NegateCooldown(itemToBuff);

                        itemState.View.Moveable.TargetPosition = itemToBuff.transform.position;
                        yield return new WaitForSeconds(0.1f);
                        itemState.View.Moveable.TargetPosition = itemState.View.ItemHolder.transform.position;

                        itemToBuff.Spin();
                        itemToBuff.UpdateTimeText();
                        break;
                    }
                }
            }
        }
    }
}