using System;
using DG.Tweening;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Source
{
    [Serializable]
    public class ItemData
    {
        public Item ItemType;
        public float BaseProbabilityWeight;
    }
    public class InteractiveObject : MonoBehaviour
    {
        public MoveableBase Moveable => _draggable.Moveable;
        public ItemHolder ItemHolder { get; set; }

        [SerializeField] private float _waveSpeed;
        [SerializeField] private float _waveAmplitude;

        [SerializeField] private GameObject _spriteHolder;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private DraggableSmoothDamp _draggable;
        [SerializeField] private bool _isLocked = false;
        
        // Im going to put the state of the item there until i figure out how to implement my verison of CMS or something like that 
        [field: SerializeField] public ItemData ItemData { get; private set; }
        private void Awake()
        {
            _draggable.OnDragEnded += CastForNewSlot;
        }

        private void Update()
        {
            if (_draggable.IsDragging)
                return;
            var waveValue = Mathf.Sin(Time.time * _waveSpeed + transform.position.x);
            _spriteHolder.transform.localPosition = new(0, waveValue * _waveAmplitude, 0);
        }
        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;
                _draggable.enabled = !value;
            } 
        }

        public void Restore()
        {
            _spriteRenderer.color = Color.white;
            transform.rotation = Quaternion.identity;
            transform.DORotate(new(0, 0, 360f), 0.2f, RotateMode.FastBeyond360).SetRelative();
        }
        public void Exhaust()
        {
            _spriteRenderer.color = Color.brown;
        }
        public void SetData(ItemData itemData)
        {
            _spriteRenderer.sprite = GameAssetReferences.Instance.GetItemSprite(itemData.ItemType);   
            ItemData = itemData;
        }
        public void CastForNewSlot(PointerEventData eventData)
        {
            var ray = Camera.main.ScreenPointToRay(eventData.position);
            var raycastHit = Physics2D.RaycastAll(ray.origin, ray.direction, 100f, 1 << 3);
            foreach (var hit in raycastHit)
            {
                var itemHolder = hit.collider.gameObject.GetComponentInParent<ItemHolder>();
                itemHolder.Claim(this);
                break;
            }
        }
        // Maybe i should pass itemtype as a parameter later on, if i want to place item data somewhere else outstide the itemview
        public void AnimateAction()
        {
            if (ItemData == null)
                return;
            switch (ItemData.ItemType)
            {
                case Item.Gun:
                    transform.DOKill();
                    transform.rotation = Quaternion.identity;
                    transform.DORotate(new(0, 0, 360f), 0.2f, RotateMode.FastBeyond360).SetRelative();
                    break;
                case Item.Bullet:
                    transform.DOKill();
                    transform.localScale = Vector3.one;
                    transform.DOPunchScale(Vector3.one, 0.2f);
                    break;
            }
        }
    }
}