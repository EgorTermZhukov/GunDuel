using Game.Source.Tags;

namespace Game.Source.Data
{
    public class BulletItem : BaseItem
    {
        public BulletItem()
        {
            Get<TagPfb>().Prefab = "Prefab/Items/Bullet".Load<InteractiveObject>();
            
            var duration = Get<TagUseDuration>(); 
            duration.BaseDuration = 1f;
            duration.Duration = 1f;
            
            Define<TagIncreaseDamage>().DamageIncrease = 1f;
            
            Get<TagName>().Name = "Bullet";
            Get<TagDescription>().Loc = "On use: Increase damage by " + Get<TagIncreaseDamage>().DamageIncrease.ToString();
        }
    }
}