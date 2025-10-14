using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Source
{
    public class PopupText : MonoBehaviour
    {
        public TMP_Text popupText;
        public SpriteRenderer SpriteRenderer;
        public float LifeTime;
        public Vector3 Velocity;
        public float Range;

        public void StartPop(string text, float duration, Color color, Sprite sprite)
        {
            SpriteRenderer.sprite = sprite;
            LifeTime = duration;
            popupText.text = text;
            popupText.color = color;
            Velocity = Vector3.up;
            StartCoroutine(SpawnRoutine());
        }
        public IEnumerator SpawnRoutine()
        {
            var endPosition = transform.position + Velocity * Range;
            transform.DOMove(endPosition, LifeTime);
            yield return new WaitUntil(G.Ticker.CreatePr(LifeTime));
            Destroy(gameObject);
        } 
    }
}