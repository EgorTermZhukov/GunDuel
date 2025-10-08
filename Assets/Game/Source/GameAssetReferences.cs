using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Source
{
    [Serializable]
    public struct ValueSprite
    {
        public int Value;
        public Sprite Sprite;
    }
    public class GameAssetReferences : MonoBehaviour
    {
        public static GameAssetReferences Instance { get; private set; }
        [SerializeField] private List<ItemSprite> _itemSprites;
        [SerializeField] private List<ValueSprite> _valueSprites;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Sprite GetItemSprite(Item itemType)
        {
            return _itemSprites.Find(x => x.Item == itemType).Sprite;
        }

        public Sprite GetValueSprite(int value)
        {
            return _valueSprites.Find(x => x.Value == value).Sprite;
        }
    }
}