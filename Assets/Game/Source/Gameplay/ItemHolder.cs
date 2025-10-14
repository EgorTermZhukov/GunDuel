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
        public event Action<InteractiveObject> OnReleased;
        public event Action<ItemHolder, InteractiveObject> OnAttemptToOccupy;
        public bool Claim(InteractiveObject interactiveObject)
        {
            if (!CanClaim)
                return false;
            if (interactiveObject == InteractiveObject)
                return false;
            if (InteractiveObject != null)
            {
                OnAttemptToOccupy?.Invoke(this, interactiveObject);
                return false;
            }
            InteractiveObject = interactiveObject;
            
            if(InteractiveObject.ItemHolder != null)
                InteractiveObject.ItemHolder.Release();
            
            InteractiveObject.Moveable.TargetPosition = transform.position;
            InteractiveObject.ItemHolder = this;
            OnClaimed?.Invoke();
            return true;
        }
        // Well release isn't really called that often lol
        public void Release()
        {
            if (InteractiveObject == null)
                return;
            
            var interactiveObject = InteractiveObject;
            // dont know about that honestly it was a placeholder all the time, maybe it's time to resolve it?
            // oh wait no it actually works okay, the only thing to resolve is the occupying of the things
            InteractiveObject.Moveable.TargetPosition = transform.position + Vector3.down * 2f;
            InteractiveObject.ItemHolder = null;
            InteractiveObject = null;
            OnReleased?.Invoke(interactiveObject);
        }
    }
}