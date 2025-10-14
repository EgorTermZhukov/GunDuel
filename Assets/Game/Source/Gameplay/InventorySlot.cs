using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Source
{
    public enum Item
    {
        None,
        Bullet
    }
    public class InventorySlot : MonoBehaviour
    {
        public Transform CursorTarget;
        public ItemHolder ItemHolder;
        public event Action OnItemClaimed;
        public event Action<InteractiveObject> OnItemReleased;
        public event Action<ItemHolder, InteractiveObject> OnAttemptToOccupy;
        // I should probably implement some kind of nullobject later
        public InteractiveObject InteractiveObject => ItemHolder.InteractiveObject;
        private void Awake()
        {
            ItemHolder = GetComponentInChildren<ItemHolder>();
            ItemHolder.OnClaimed += HolderClaimed;
            ItemHolder.OnReleased += HolderReleased;
            ItemHolder.OnAttemptToOccupy += TriesToOccupy;
        }
        public void Claim(InteractiveObject interactiveObject)
        {
            if (InteractiveObject == interactiveObject)
                return;
            // Overrides Item holders claim and calls it explicitly
            ItemHolder.Claim(interactiveObject);
            OnItemClaimed?.Invoke();
        }
        public void Release()
        {
            ItemHolder.Release();
        }
        private void HolderClaimed()
        {
            OnItemClaimed?.Invoke();
        }
        private void HolderReleased(InteractiveObject interactiveObject)
        {
            OnItemReleased?.Invoke(interactiveObject);
        }

        private void TriesToOccupy(ItemHolder itemHolder, InteractiveObject interactiveObject)
        {
            OnAttemptToOccupy?.Invoke(itemHolder, interactiveObject);
        }
    }
}