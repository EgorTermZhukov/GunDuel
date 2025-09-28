using UnityEngine;

namespace Game.Source
{
    public class DiceRoller
    {
        public int MinValue = 1;
        public int MaxValue = 6;
        public void Roll(SlotArea slotArea)
        {
            int value = UnityEngine.Random.Range(MinValue, MaxValue);
            var slotsWithCorrValue = slotArea.GetSlotsWithCorrespondingValue(value);
            foreach (var slot in slotsWithCorrValue)
            {
                ExecuteItem(slot.SlotItem);
                slot.PopScale();
            }
        }
        public void ExecuteItem(SlotItem item)
        {
            switch (item)
            {
                case SlotItem.Attack:
                    Debug.Log("Attack");
                    break;
                case SlotItem.Defense:
                    Debug.Log("Defense");
                    break;
            }
        }
    }
}