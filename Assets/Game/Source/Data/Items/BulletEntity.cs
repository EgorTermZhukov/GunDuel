using Game.Source.Tags;

namespace Game.Source.Data
{
    public class BulletEntity : BaseItem
    {
        public BulletEntity()
        {
            Get<TagPfb>().Prefab = "Prefab/Items/Bullet".Load<InteractiveObject>();
            Get<TagUseDuration>().BaseDuration = 0.2f;
            Get<TagUseDuration>().Duration = 1f;
            Define<TagIncreaseDamage>().DamageIncrease = 1f;
        }
    }
}