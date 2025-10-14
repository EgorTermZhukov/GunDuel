using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Source.Data;
using Game.Source.Tags;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Source
{
    public class ItemState
    {
        public CMSEntity Model;
        public List<ModifiableComponentDefinition> ModifiableComponents;
        public float BaseUseDuration;
        public float UseDuration;
        public bool Exhausted;
        public InteractiveObject View;
        
        public T Get<T>() where T : EntityComponentDefinition, new()
        {
            return ModifiableComponents.Find(m => m is T) as T;
        }
        public bool Is<T>(out T unknown) where T : ModifiableComponentDefinition, new()
        {
            unknown = Get<T>();
            return unknown != null;
        }
    }
    public class InteractiveObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public ItemState ItemState;
        
        [SerializeField] private DraggableSmoothDamp _draggable;

        public event Action<PointerEventData, InteractiveObject> OnDragEnded;

        public MoveableBase Moveable => _draggable.Moveable;
        public ItemHolder ItemHolder { get; set; }

        [SerializeField] private TMP_Text _useText;

        [SerializeField] private GameObject _spriteHolder;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private SpriteRenderer _levelRenderer;
        
        [SerializeField] private bool _isLocked = false;
        [SerializeField] private bool _timerTicking = false;
        [SerializeField] private float _useTimer = 0f;
        
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
            _draggable.OnDragEnded += FinishedDragging;
            OnDragEnded += G.main.CastForNewSlot;
        }

        private void FinishedDragging(PointerEventData pointer)
        {
            OnDragEnded?.Invoke(pointer, this);
        }

        private void OnDestroy()
        {
            _draggable.OnDragEnded -= FinishedDragging;
            OnDragEnded -= G.main.CastForNewSlot;
        }
        private void Update()
        {
            var colorTag = ItemState.Model.Get<TagColorPaletteProvider>();
            _colorT = Mathf.Sin(transform.position.x + Time.time * _colorCycleSpeed);
 
            if(!ItemState.Exhausted)
                _spriteRenderer.color = Color.Lerp(colorTag.BaseColor, colorTag.MaxColor, _colorT);
        }
        public IEnumerator StartTickingTimer()
        {
            var tickable = G.Ticker.Create(ItemState.Get<TagUseDuration>().Duration);
            StartCoroutine(StartTimer(tickable));
            yield break;
        }
        public IEnumerator StartTimer(Tickable tickable)
        {
            while (!tickable.Finished)
            {
                var roundedValue = Math.Round(tickable.Duration, 1);
                _useText.text = roundedValue.ToString();
                yield return null;
            }
            yield break;
        }
        public void UpdateLevel()
        {
            var level = ItemState.Get<TagItemLevel>();
            
            var baseDuration = ItemState.Model.Get<TagBaseUseDuration>().Get(level);
            ItemState.Get<TagUseDuration>().Duration = baseDuration;
            
            var sprites = ItemState.Model.Get<TagLevelSprites>().LevelSprites;

            var spriteToSet = sprites[level.Level];
            _levelRenderer.sprite = spriteToSet;
            _levelRenderer.gameObject.transform.DOPunchScale(new(0.2f, 0.2f, 0.2f), 0.2f);
            UpdateTimeText();
        }
        public void UpdateTimeText()
        {
            _useText.transform.DOKill();
            _useText.transform.localScale = Vector3.one;
            _useText.text = ItemState.Get<TagUseDuration>().Duration.ToString();
            _useText.gameObject.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
        }
        public void SetState(ItemState itemState)
        {
            ItemState = itemState;
            UpdateTimeText();
        }
        public void Restore()
        {
            ItemState.Exhausted = false;
            _spriteRenderer.color = Color.white;
            Spin();
        }
        public void Spin(float duration = 0.4f)
        {
            // TODO Use ticker here
            _spriteHolder.transform.DOKill();
            _spriteHolder.transform.rotation = Quaternion.identity;
            _spriteHolder.transform.DORotate(new(0, 0, 360f), duration, RotateMode.FastBeyond360).SetRelative();
        }
        public void Exhaust()
        {
            _spriteHolder.transform.DOKill();
            _spriteHolder.transform.localScale = Vector3.one;
            _spriteHolder.transform.DOPunchScale(new(0.2f, 0.2f, 0.2f), 0.2f);
            ItemState.Exhausted = true;
            //_spriteRenderer.color = Color.brown;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_draggable.IsDragging)
                return;
            var name = ItemState.Model.Get<TagName>();
            // this probably should do it in another manner
            var description = ItemState.Get<TagDescription>();
            G.main.Tooltip.Show(name.Name, description.Loc);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            G.main.Tooltip.Hide();
        }

        public void SlashTowards(GameObject target)
        {
            StartCoroutine(SlashTowardsRoutine(target));
        }
        public IEnumerator SlashTowardsRoutine(GameObject target)
        {
            Spin(0.5f);
            transform.position = target.transform.position;
            
            Moveable.TargetPosition = target.transform.position;
            
            _spriteHolder.transform.DOPunchScale(new(0.2f, 0.2f, 0.2f), 0.2f);
            G.ParticleController.Spawn(target.transform.position, ParticleType.Slash);
            yield return new WaitUntil(G.Ticker.CreatePr(0.5f));
            Moveable.TargetPosition = ItemHolder.transform.position;
            _spriteHolder.transform.DOKill();
            _spriteHolder.transform.rotation = Quaternion.identity;
        }
        public void MoveToAndBack(Vector3 position, float duration)
        {
            StartCoroutine(MoveToAndBackRoutine(position, duration));
        }
        public IEnumerator MoveToAndBackRoutine(Vector3 position, float duration)
        {
            Moveable.TargetPosition = position;
            if (duration == 0)
            {
                transform.position = Moveable.TargetPosition;
            }
            yield return new WaitUntil(G.Ticker.CreatePr(duration));
            Moveable.TargetPosition = ItemHolder.transform.position;
        }
    }
}