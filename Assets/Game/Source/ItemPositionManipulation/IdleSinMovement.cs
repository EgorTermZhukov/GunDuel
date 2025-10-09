using System;
using UnityEngine;

namespace Game.Source
{
    public class IdleSinMovement : MonoBehaviour
    {
        [SerializeField] private GameObject _spriteHolder;
        
        [SerializeField] private DraggableSmoothDamp _draggable;

        [SerializeField] private float _waveSpeed;
        [SerializeField] private float _waveAmplitude;
        private void Update()
        {
            if (_draggable.IsDragging)
                return;
            var waveValue = Mathf.Sin(Time.time * _waveSpeed + transform.position.x);
            _spriteHolder.transform.localPosition = new(0, waveValue * _waveAmplitude, 0);
        }
    }
}