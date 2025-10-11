using System;
using Game.Source.Tags;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Source
{
    public class ItemHolder : MonoBehaviour
    {
        // later on use this to do something i dunno
        public bool CanClaim = true;
        public InteractiveObject InteractiveObject;
        
        // so from what i understand, events should be used in the cases where you dont want some bit to know what it's used for
        public event Action OnClaimed;
        public event Action OnReleased;
        public bool Claim(InteractiveObject interactiveObject)
        {
            if (!CanClaim)
                return false;
            if (interactiveObject == InteractiveObject)
                return false;
            if(InteractiveObject != null)
                Release();
            
            InteractiveObject = interactiveObject;
            
            if(InteractiveObject.ItemHolder != null)
                InteractiveObject.ItemHolder.Release();
            
            InteractiveObject.Moveable.TargetPosition = transform.position;
            InteractiveObject.ItemHolder = this;
            OnClaimed?.Invoke();
            return true;
        }
        public void Release()
        {
            if (InteractiveObject == null)
                return;
            // Call main to resolve a stray itemview
            InteractiveObject.Moveable.TargetPosition = transform.position + Vector3.down * 2f;
            InteractiveObject.ItemHolder = null;
            InteractiveObject = null;
            OnReleased?.Invoke();
        }
    }
}