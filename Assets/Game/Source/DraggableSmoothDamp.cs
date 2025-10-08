using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Source
{
    public class DraggableSmoothDamp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public MoveableBase Moveable;
        // maybe extract this to another component later on
        public event Action<PointerEventData> OnDragEnded;
        
        public bool IsDragging { get; private set; }
        
        private Camera _mainCamera;
        
        private Vector2 _origin;
        private Vector3 _offset;

        private void Start()
        {
            _mainCamera = Camera.main;
            //Moveable.TargetPosition = transform.position;
        }
        public void Update()
        {
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            IsDragging = true;
            _origin = Moveable.TargetPosition;
            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(transform.position);
            _offset = transform.position -
                      _mainCamera.ScreenToWorldPoint(new(eventData.position.x, eventData.position.y, 0));
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            IsDragging = false;
            Moveable.TargetPosition = _origin;
            OnDragEnded?.Invoke(eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            Vector3 cursorPoint = new Vector3(eventData.position.x, eventData.position.y, 0);
            Vector3 cursorPosition = _mainCamera.ScreenToWorldPoint(cursorPoint);
            cursorPosition.z = transform.position.z;
            Moveable.TargetPosition = cursorPosition;
            // scan for slots, if nothing has been hit then return to the previous position
        }
    }
}