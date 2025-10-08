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
    }
    [Serializable]
    public struct ItemDataInitialization
    {
        public Item ItemType;
    }
    public class Main : MonoBehaviour
    {
        [SerializeField] public SlotArea PlayerArea;
        [SerializeField] public SlotArea EnemyArea;
        
        [SerializeField] InteractiveObject interactiveObjectPrefab;
        
        // debug parts for now
        public List<ItemDataInitialization> _itemData = new List<ItemDataInitialization>();
        public List<SlotInitializationDebug> _debugSlots = new List<SlotInitializationDebug>();
        private void Awake()
        {
            G.main = this;
        }
        private void Start()
        { 
            foreach (var slot in _debugSlots)
            {
                var faceSlot = PlayerArea.AddSlot();
                
                if (slot.Item == Item.None)
                    continue;
                
                var itemView = Instantiate(interactiveObjectPrefab).GetComponent<InteractiveObject>();
                var itemData = _itemData.Find(x => x.ItemType == slot.Item);
                itemView.SetData(new (){ItemType = itemData.ItemType});
                
                faceSlot.Claim(itemView);
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                var itemView = Instantiate(interactiveObjectPrefab).GetComponent<InteractiveObject>();
                var itemData = _itemData.Find(x => x.ItemType == Item.Bullet);
                itemView.SetData(new ItemData() {ItemType = itemData.ItemType});
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