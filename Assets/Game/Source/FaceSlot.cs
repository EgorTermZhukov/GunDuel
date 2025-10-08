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
        Gun,
        Bullet
    }
    public class FaceSlot : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _valueRenderer;
        [SerializeField] private TMP_Text _probabilityText;
        
        public ItemHolder ItemHolder;
        public event Action OnItemClaimed;

        public event Action OnItemReleased;
        // I should probably implement some kind of nullobject later
        public InteractiveObject InteractiveObject => ItemHolder.InteractiveObject;
        public float ProbabilityPercentage = 0f;
        public float Weight = 0f;
        private int _value;

        [field: SerializeField]
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueRenderer.sprite = GameAssetReferences.Instance.GetValueSprite(value);
            }
        }
        private void Awake()
        {
            ItemHolder = GetComponentInChildren<ItemHolder>();
            ItemHolder.OnClaimed += HolderClaimed;
            ItemHolder.OnReleased += HolderReleased;
        }

        public void SetProbabilityText(float probabiltiyPercentage)
        {
            if (probabiltiyPercentage == ProbabilityPercentage)
                return;
            _probabilityText.transform.DOKill();
            _probabilityText.gameObject.transform.localScale = Vector3.one;
            _probabilityText.gameObject.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0f), 0.2f);
            var roundedPercentage = Math.Round(probabiltiyPercentage, 2);
            _probabilityText.text = roundedPercentage.ToString() + "%";
            ProbabilityPercentage = probabiltiyPercentage;
        }
        // wrappers for item holder inside
        public void Claim(InteractiveObject interactiveObject)
        {
            if (InteractiveObject == interactiveObject)
                return;
            // Overrides Item holders claim and calls it explicitly
            ItemHolder.Claim(interactiveObject);
            OnItemClaimed?.Invoke();
            Debug.Log("Slot with value " + Value + " Claimed " + interactiveObject.ItemData.ItemType);
        }
        public void Release()
        {
            ItemHolder.Release();
        }
        private void HolderClaimed()
        {
            OnItemClaimed?.Invoke();
        }
        private void HolderReleased()
        {
            OnItemReleased?.Invoke();
        }
        public void Activate()
        {
            if (InteractiveObject == null)
                return;
            InteractiveObject.AnimateAction();
        }
    }
}