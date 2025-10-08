using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Source
{
    [Serializable]
    public struct SlotInitializationDebug
    {
        public int DiceValue;
        public Item Item;
        public float BaseWeight;
    }
    [Serializable]
    public struct ItemDataInitialization
    {
        public Item ItemType;
        public float Weight;
    }
    public class Main : MonoBehaviour
    {
        [SerializeField] private SlotArea _slotArea;
        [SerializeField] InteractiveObject interactiveObjectPrefab;
        private DiceRoller _diceRoller;
        
        // debug parts for now
        public List<ItemDataInitialization> _itemData = new List<ItemDataInitialization>();
        public List<SlotInitializationDebug> _debugSlots = new List<SlotInitializationDebug>();
         private void Awake()
        {
  
        }
        private void Start()
        { 
            _diceRoller = new DiceRoller();
            foreach (var slot in _debugSlots)
            {
                var faceSlot = _slotArea.AddSlot(slot.DiceValue, slot.BaseWeight);
                
                if (slot.Item == Item.None)
                    continue;
                
                var itemView = Instantiate(interactiveObjectPrefab).GetComponent<InteractiveObject>();
                var itemData = _itemData.Find(x => x.ItemType == slot.Item);
                itemView.SetData(new (){ItemType = itemData.ItemType, BaseProbabilityWeight = itemData.Weight});
                
                faceSlot.Claim(itemView);
            }
            _slotArea.RecalculateProbabilities();
        }
        private void Update()
        {
            // debug
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _diceRoller.Roll(_slotArea);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                var itemView = Instantiate(interactiveObjectPrefab).GetComponent<InteractiveObject>();
                var itemData = _itemData.Find(x => x.ItemType == Item.Gun);
                itemView.SetData(new ItemData() {ItemType = itemData.ItemType, BaseProbabilityWeight = itemData.Weight});
                itemView.Moveable.TargetPosition = Vector2.down * 4 + Vector2.right;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                var itemView = Instantiate(interactiveObjectPrefab).GetComponent<InteractiveObject>();
                var itemData = _itemData.Find(x => x.ItemType == Item.Bullet);
                itemView.SetData(new ItemData() {ItemType = itemData.ItemType, BaseProbabilityWeight = itemData.Weight});
                itemView.Moveable.TargetPosition = Vector2.down * 4 + Vector2.right;
            }
        }
        private IEnumerator StartGameLoop()
        {
            while (true)
            {
                yield return null;
            }
        }
    }
}