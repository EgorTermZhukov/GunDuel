using System.Collections;
using System.Collections.Generic;
using Game.Source.Tags;
using Game.Source.Utils;
using UnityEngine;

namespace Game.Source.Data
{
    public class KnifeItem : BaseItem
    {
        public KnifeItem()
        {
            Get<TagPfb>().Prefab = "Prefab/Items/Knife".Load<InteractiveObject>();
            
            var baseDuration = Define<TagBaseUseDuration>();
            var level = Get<TagItemLevel>();

            baseDuration.Duration = new List<float>(level.MaxLevel + 1)
            {
                0.6f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f
            };
            
            var duration = Define<TagUseDuration>().Duration = baseDuration.Get(level); 

            Define<TagDealFlatDamage>().Damage = new List<float>(level.MaxLevel + 1)
            {
                2f, 2.5f, 3f, 3.5f, 4f, 4.5f
            };
            
            Get<TagName>().Name = "Knife";
            Get<TagDescription>().Loc = "On use: Deal flat " + Get<TagDealFlatDamage>().Damage + " damage";
        }
    }

    public class TagDealFlatDamage : ModifiableComponentDefinition
    {
        public List<float> Damage;

        public float Get(TagItemLevel level) => Damage[level.Level];
    }

    public class DealFlatDamageInteraction : BaseInteraction, IOnUse
    {
        public IEnumerator OnUse(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea)
        {
            if (itemState.Is<TagDealFlatDamage>(out var dfd))
            {
                var level = itemState.Get<TagItemLevel>();
                var duration = itemState.Get<TagUseDuration>();
                
                yield return new WaitUntil(G.Ticker.CreatePr(duration.Duration));
                
                itemState.View.SlashTowards(sideTurns.OpposingSideTurns.CharacterView.gameObject);
                yield return sideTurns.OpposingSideTurns.TakeDamage(dfd.Get(level));
            }
        }
    }
    public class DealFlatDamageDescription : IDescriptionProvider
    {
        public string GetDescription(ItemState state)
        {
            if (state.Model.Is<TagDealFlatDamage>(out var tag))
            {
                var level = state.Get<TagItemLevel>();
                var value = tag.Get(level);
                string description = $"On use: deals <color=red> {value} </color> flat damage to the opponent";
                return description;
            }
            return "";
        }
    }
}