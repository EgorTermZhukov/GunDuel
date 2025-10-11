using Game.Source.Tags;

namespace Game.Source.Data.Characters
{
    public class SideState : EntityComponentDefinition
    {
        public float Health;
        public float Damage;
        public float DamageMultiplier;
        public float Defense;
        public float DefenseMultiplier;
    }

    public class TagSlotCount : EntityComponentDefinition
    {
        public int Count;
    }
    public class BasicCharacter : CMSEntity
    {
        public BasicCharacter()
        {
            var state = Define<SideState>();
            state.Health = 100f;
            state.Damage = 0;
            state.DamageMultiplier = 1f;
            state.Defense = 0f;
            state.DefenseMultiplier = 1f;
            Define<TagSlotCount>().Count = 6;
        }
    }
}