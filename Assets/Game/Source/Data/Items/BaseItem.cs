using System.Collections.Generic;
using Game.Source.Tags;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Source.Data
{
    public class BaseItem : CMSEntity
    {
        public BaseItem()
        {
            Define<TagPfb>().Prefab =  "Prefab/Items/InteractiveObject".Load<InteractiveObject>();

            Define<TagName>().Name = "BaseItem";
            Define<TagDescription>().Loc = "Does nothing";
            
            Define<TagUsed>().Used = false;

            var level = Define<TagItemLevel>();

            level.Level = 0;
            level.MaxLevel = 6;
            
            var rarity = Define<TagRarity>();
            rarity.Rarity = 1;
            rarity.Weight = 1f;

            var levelSprites = Define<TagLevelSprites>().LevelSprites = new List<Sprite>();
            
            levelSprites.Add(GetSprite());
            levelSprites.Add(CMSUtil.LoadFromSpritesheet("Graphics/GraphicsAtlas", "Tier_1"));
            levelSprites.Add(CMSUtil.LoadFromSpritesheet("Graphics/GraphicsAtlas", "Tier_2"));
            levelSprites.Add(CMSUtil.LoadFromSpritesheet("Graphics/GraphicsAtlas", "Tier_3"));
            levelSprites.Add(CMSUtil.LoadFromSpritesheet("Graphics/GraphicsAtlas", "Tier_4"));
            levelSprites.Add(CMSUtil.LoadFromSpritesheet("Graphics/GraphicsAtlas", "Tier_5"));
            
            Define<TagColorPaletteProvider>().BaseColor = Color.white;
            Get<TagColorPaletteProvider>().MaxColor = Color.gray7;
        }
    }
}