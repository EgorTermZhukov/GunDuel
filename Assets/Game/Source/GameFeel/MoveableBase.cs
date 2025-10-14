using System;
using UnityEngine;

namespace Game.Source
{
    public class MoveableBase : MonoBehaviour
    {
        public Vector3 TargetPosition;
        public float LerpSpeed = 1f;
        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, LerpSpeed * Time.deltaTime * G.Ticker.TimeScale);
        }
    }
}