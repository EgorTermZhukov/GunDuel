using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Source
{
    [Serializable]
    public struct SlotInitializationDebug
    {
        public int DiceValue;
        public SlotItem SlotItem;
    }
    public class Main : MonoBehaviour
    {
        [SerializeField] private SlotArea _slotArea;
        private DiceRoller _diceRoller;
        
        public List<SlotInitializationDebug> _debugSlots = new List<SlotInitializationDebug>();
        
        private void Awake()
        {
            _diceRoller = new DiceRoller();
            foreach (var slot in _debugSlots)
            {
                _slotArea.AddSlot(slot.DiceValue, slot.SlotItem);
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _diceRoller.Roll(_slotArea);
            }
        }
    }
}