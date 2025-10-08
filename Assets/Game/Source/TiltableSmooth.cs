using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Source
{
    public class TiltableSmooth : MonoBehaviour
    {
        public GameObject Tiltable;
        
        public float MaxTiltAngle = 45f;
        public float TiltIntensity = 5f;
        public float SmoothTime = 0.1f;

        private Vector3 _lastFramePosition;
        private float _currentTilt;
        private float _tiltVelocity;

        private void Start()
        {
            _lastFramePosition = transform.position;
        }

        // What i learned is that quaternions are dirty cheap actually and there is no reason to optimize them
        // The actual thing here is transform writes lool
        private void LateUpdate()
        {
            var delta = transform.position - _lastFramePosition;
            var targetTilt = Mathf.Clamp((delta.x + delta.y)  * TiltIntensity, -MaxTiltAngle, MaxTiltAngle);
            
            _currentTilt = Mathf.SmoothDamp(_currentTilt, targetTilt, ref _tiltVelocity, SmoothTime);

            Tiltable.transform.localRotation = Quaternion.Euler(0f, 0f, _currentTilt);

            _lastFramePosition = transform.position;
        }
    }
}