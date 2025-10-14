using System.Collections;
using System.Collections.Generic;
using Game.Source.Tags;

namespace Game.Source.Data
{
    public class BulletItem : BaseItem
    {
        public BulletItem()
        {
            Get<TagPfb>().Prefab = "Prefab/Items/Bullet".Load<InteractiveObject>();

            var baseDuration = Define<TagBaseUseDuration>();
            var level = Get<TagItemLevel>();

            baseDuration.Duration = new List<float>(level.MaxLevel + 1)
            {
                1f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f
            };
            
            var duration = Define<TagUseDuration>().Duration = baseDuration.Get(level); 

            Define<TagIncreaseDamage>().DamageIncrease = new List<float>(level.MaxLevel + 1)
            {
                1f, 1.25f, 1.50f, 1.75f, 2f, 2.25f
            };
            
            Get<TagName>().Name = "Bullet";
            // provide description i dunno
            Get<TagDescription>().Loc = "On use: Increase damage by " + Get<TagIncreaseDamage>().DamageIncrease.ToString();
        }
    }
}