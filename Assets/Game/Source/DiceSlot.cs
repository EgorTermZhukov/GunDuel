using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game.Source
{
    public enum SlotItem
    {
        None,
        Attack,
        Defense
    }
    public class DiceSlot : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _itemRenderer; 
        public SlotItem SlotItem; 
        public int Value;
        private void Start()
        {
        }
        public void Initialize(int value, SlotItem slotItem, List<SlotItemSprite> slotItemSprites)
        {
            Value = value;
            SlotItem = slotItem;
            var sprite = slotItemSprites.Find(x => x.SlotItem == slotItem).Sprite;
            _itemRenderer.sprite = sprite;
        }
        public void PopScale()
        {
            _itemRenderer.gameObject.transform.DOPunchScale(Vector3.one, 0.2f);
        }
    }
}