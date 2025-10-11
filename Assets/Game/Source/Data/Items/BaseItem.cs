using Game.Source.Tags;
using UnityEngine;

namespace Game.Source.Data
{
    public class BaseItem : CMSEntity
    {
        public BaseItem()
        {
            Define<TagPfb>().Prefab =  "Prefab/Items/InteractiveObject".Load<InteractiveObject>();

            Define<TagName>().Name = "BaseItem";
            Define<TagDescription>().Loc = "Does nothing";
            
            Define<TagUseDuration>().BaseDuration = 1f;
            Get<TagUseDuration>().Duration = 1f;
            
            Define<TagUsed>().Used = false;

            Define<TagItemLevel>().Level = 1;

            var rarity = Define<TagRarity>();
            rarity.Rarity = 1;
            rarity.Weight = 1f;
            
            Define<TagColorPaletteProvider>().BaseColor = Color.white;
            Get<TagColorPaletteProvider>().MaxColor = Color.gray7;

        }
    }
}