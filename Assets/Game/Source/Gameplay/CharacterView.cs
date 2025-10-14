using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Source.Tags;
using UnityEngine;

namespace Game.Source
{
    public class CharacterState
    {
        public CMSEntity Model;
        public List<ModifiableComponentDefinition> ModifiableComponents;
        
        // tags, model, view
        public CharacterView View;
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
    public class CharacterView : MonoBehaviour
    {
        public CharacterState CharacterState;

        public SpriteRenderer Sprite;
        
        public Color BaseColor;
        public Color FlashColor;
        
        public Transform Weapon;
        public Transform StatIncreaseTarget;
        public Transform StatDecreaseTarget;

        public void SetState(CharacterState state)
        {
        }
        public void Flash()
        {
            StopAllCoroutines();
            StartCoroutine(FlashRoutine());
        }
        public IEnumerator FlashRoutine()
        {
            var duration = 0.1f;
            Sprite.DOColor(FlashColor, duration);
            yield return new WaitUntil(G.Ticker.CreatePr(duration));
            Sprite.DOColor(BaseColor, duration);
            yield return new WaitUntil(G.Ticker.CreatePr(duration));
            Sprite.DOColor(FlashColor, duration);
            yield return new WaitUntil(G.Ticker.CreatePr(duration));
            Sprite.DOColor(BaseColor, duration);
            yield return new WaitUntil(G.Ticker.CreatePr(duration));
        }
    }
}