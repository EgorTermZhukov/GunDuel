using System;
using System.Collections;
using Game.Source.Data.Characters;

namespace Game.Source.Tags
{
    [Serializable]
    public class TagIncreaseDamage : ModifiableComponentDefinition
    {
        public float DamageIncrease;
    }
    // i dont know if i am going to store side state in slot area yet so im just passing it as a paremeter, wont hurt
    public class IncreaseDamageFlat : BaseInteraction, IOnUse
    {
        public IEnumerator OnUse(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea)
        {
            // temporary use model, later get certain stuff i dunno
            if (itemState.Model.Is<TagIncreaseDamage>(out var dmg))
            {
                yield return sideTurns.IncreaseDamage(dmg.DamageIncrease, sideTurns.SideState);
            }
        }
    }
    public interface IOnUse
    {
        public IEnumerator OnUse(ItemState itemState, SideTurnsManager sideTurns, SlotArea slotArea);
    }
}