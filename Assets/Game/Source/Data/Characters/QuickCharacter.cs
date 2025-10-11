namespace Game.Source.Data.Characters
{
    public class QuickCharacter : CMSEntity
    {
        public QuickCharacter()
        {
            var state = Define<SideState>();
            state.Health = 10f;
            state.Damage = 2;
            state.DamageMultiplier = 1.2f;
            state.Defense = 0f;
            state.DefenseMultiplier = 1f;
            Define<TagSlotCount>().Count = 3;
        }
    }
}