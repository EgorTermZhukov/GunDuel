using System.Collections;
using Game.Source.Tags;
using UnityEngine;

namespace Game.Source.Data
{
    public class KnifeItem : BaseItem
    {
        public KnifeItem()
        {
            Get<TagPfb>().Prefab = "Prefab/Items/Knife".Load<InteractiveObject>();
            
            Get<TagUseDuration>().BaseDuration = 0.3f;
            Get<TagUseDuration>().Duration = 0.3f;
            
            Define<TagDealFlatDamage>().Damage = 2f;
            
            Get<TagName>().Name = "Knife";
            Get<TagDescription>().Loc = "On use: Deal flat " + Get<TagDealFlatDamage>().Damage + " damage";
        }
    }

    public class TagDealFlatDamage : ModifiableComponentDefinition
    {
        public float Damage;
    }

    public class DealFlatDamageInteraction : BaseInteraction, IOnUse
    {
        public IEnumerator OnUse(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea)
        {
            if (itemState.Is<TagDealFlatDamage>(out var dfd))
            {
                itemState.View.SlashTowards(sideTurns.OpposingSideTurns.SideCharacter);
                yield return sideTurns.OpposingSideTurns.TakeDamage(dfd.Damage);
            }
        }
    }
}