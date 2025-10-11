using System;
using UnityEngine;

namespace Game.Source
{
    public class ItemDestroyer : MonoBehaviour
    {
        [SerializeField] private ItemHolder _itemHolder;
        private void Awake()
        {
            _itemHolder.OnClaimed += DestroyItem;
        }
        public void DestroyItem()
        {
            if (_itemHolder.InteractiveObject == null)
                return;
            var itemToDestroy = _itemHolder.InteractiveObject.gameObject;
            _itemHolder.Release();
            Destroy(itemToDestroy);
            _itemHolder.InteractiveObject = null;
        }
    }
}