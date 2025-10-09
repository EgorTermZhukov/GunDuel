using System;
using DG.Tweening;
using Game.Source.Tags;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Source
{
    public class ItemState
    {
        public CMSEntity Model;
        public float BaseUseDuration;
        public float UseDuration;
        public bool Exhausted;
        public InteractiveObject View;
    }
    public class InteractiveObject : MonoBehaviour
    {
        public ItemState ItemState;
        
        [SerializeField] private DraggableSmoothDamp _draggable;
        public MoveableBase Moveable => _draggable.Moveable;
        public ItemHolder ItemHolder { get; set; }

        [SerializeField] private TMP_Text _useText;

        [SerializeField] private GameObject _spriteHolder;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [SerializeField] private bool _isLocked = false;
        
        [SerializeField] private float _colorCycleSpeed = 2f;

        private float _colorT = 0f;
        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;
                _draggable.enabled = !value;
            }
        }
        private void Awake()
        {
            _draggable.OnDragEnded += CastForNewSlot;
        }
        // move this somewhere else later
        private void Update()
        {
            var colorTag = ItemState.Model.Get<TagColorPaletteProvider>();
            _colorT = Mathf.Sin(transform.position.x + Time.time * _colorCycleSpeed);
 
            if(!ItemState.Exhausted)
                _spriteRenderer.color = Color.Lerp(colorTag.BaseColor, colorTag.MaxColor, _colorT);
        }

        public void SetUseTimeText(float useTime)
        {
            _useText.text = useTime.ToString();
        }
        public void SetState(ItemState itemState)
        {
            ItemState = itemState;
            SetUseTimeText(itemState.Model.Get<TagUseDuration>().Duration);
            // receive some sort of palette changer here that constantly updates the colors of an object
        }
        public void Restore()
        {
            ItemState.Exhausted = false;
            _spriteRenderer.color = Color.white;
            transform.rotation = Quaternion.identity;
            transform.DORotate(new(0, 0, 360f), 0.2f, RotateMode.FastBeyond360).SetRelative();
        }
        public void Exhaust()
        {
            ItemState.Exhausted = true;
            _spriteRenderer.color = Color.brown;
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
    }
}