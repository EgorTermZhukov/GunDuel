using System;
using System.Collections;
using System.Collections.Generic;
using Game.Source.Data;
using Game.Source.Data.Characters;
using Game.Source.Tags;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Source
{
    [Serializable]
    public struct SlotInitializationDebug
    {
        public string EntityID;
    }
    [Serializable]
    public struct ItemDataInitialization
    {
        public Item ItemType;
    }
    public class Main : MonoBehaviour
    {
        // maybe SideManager should be in SlotArea...
        [SerializeField] public SlotArea PlayerArea;
        [SerializeField] public SlotArea EnemyArea;

        public Interactor Interactor;
        public SideManager PlayerSide;
        
        // debug parts for now
        public List<SlotInitializationDebug> _debugSlots = new List<SlotInitializationDebug>();
        private void Awake()
        {
            Interactor = new Interactor();
            Interactor.Init();
            
            G.main = this;
        }
        private void Start()
        { 
            CMS.Init();

            //G.OnGameReady?.Invoke();
            var basicCharacter = CMS.Get<BasicCharacter>();
            PlayerSide = new SideManager(basicCharacter.Get<SideState>());
            
            foreach (var slot in _debugSlots)
            {
                var faceSlot = PlayerArea.AddSlot();
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(AddItem<BulletEntity>());
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(UseAllSequence(PlayerArea));
            }
        }

        public IEnumerator UseAllSequence(SlotArea slotArea)
        {
            foreach (var slot in slotArea.FaceSlots)
            {
                if (slot.InteractiveObject == null)
                    yield return null;
                var onUsed = Interactor.FindAll<IOnUse>();
                var item = slot.InteractiveObject;
                foreach (var onUse in onUsed)
                {
                    yield return onUse.OnUse(item.ItemState, PlayerSide, PlayerArea);
                    yield return new WaitForSeconds(item.ItemState.Model.Get<TagUseDuration>().Duration);
                }
            }
        }
        public IEnumerator AddItem<T>() where T : CMSEntity
        {
            var basicItem = CMS.Get<T>();
            Debug.Log(basicItem.id);
            var interactiveObject = Instantiate(basicItem.Get<TagPfb>().Prefab);

            var state = new ItemState();
            state.Model = basicItem;
            state.View = interactiveObject;
            
            interactiveObject.SetState(state);
            
            PlayerArea.AddItemToFreeSlot(interactiveObject);
            
            yield break;
        }
    }
}