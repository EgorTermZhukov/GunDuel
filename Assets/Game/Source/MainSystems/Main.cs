using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Source.Data;
using Game.Source.Data.Characters;
using Game.Source.Tags;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField] public Tooltip Tooltip;
        
        [SerializeField] public SlotArea PlayerSlots;
        [SerializeField] public SlotArea EnemySlots;

        [SerializeField] public StatsView PlayerStatsView;
        [SerializeField] public StatsView EnemyStatsView;

        [SerializeField] public CharacterView PlayerCharacter;
        [SerializeField] public CharacterView EnemyCharacter;

        [SerializeField] private TMP_Text _winText;

        public Interactor Interactor;
        public SideTurnsManager PlayerSideTurns;
        public SideTurnsManager EnemySideTurns;

        public bool FightHappening = false;
        public bool SomebodyLost = false;
        
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
            var playerCharacter = CMS.Get<BasicCharacter>();
            var enemyCharacter = CMS.Get<BasicCharacter>();
            
            PlayerSideTurns = new SideTurnsManager(playerCharacter.Get<SideState>(), PlayerStatsView, PlayerSlots, PlayerCharacter);
            EnemySideTurns = new SideTurnsManager(enemyCharacter.Get<SideState>(), EnemyStatsView, EnemySlots, EnemyCharacter);
            
            PlayerSideTurns.OpposingSideTurns = EnemySideTurns;
            EnemySideTurns.OpposingSideTurns = PlayerSideTurns;

            for (int i = 0; i < playerCharacter.Get<TagSlotCount>().Count; i++)
            {
                var faceSlot = PlayerSlots.AddSlot();
            }
            for (int i = 0; i < enemyCharacter.Get<TagSlotCount>().Count; i++)
            {
                var faceSlot = EnemySlots.AddSlot();
            }
        }
        public IEnumerator AnnounceWinner(string winner)
        {
            _winText.text = "Winner - " + winner;
            _winText.transform.gameObject.SetActive(true);
            _winText.transform.DOShakePosition(2.5f);
            yield return new WaitForSeconds(3f);
            _winText.transform.gameObject.SetActive(false);
            ResetAllSideStats();
            FightHappening = false;
        }
        public void ResetAllSideStats()
        {
            var playerCharacter = CMS.Get<BasicCharacter>();
            var enemyCharacter = CMS.Get<BasicCharacter>();
            
            PlayerSideTurns = new SideTurnsManager(playerCharacter.Get<SideState>(), PlayerStatsView, PlayerSlots, PlayerCharacter);
            StartCoroutine(PlayerSideTurns.RestoreSlots());
            EnemySideTurns = new SideTurnsManager(enemyCharacter.Get<SideState>(), EnemyStatsView, EnemySlots, EnemyCharacter);
            StartCoroutine(EnemySideTurns.RestoreSlots());
            
            PlayerSideTurns.OpposingSideTurns = EnemySideTurns;
            EnemySideTurns.OpposingSideTurns = PlayerSideTurns;
        }
        private void Update()
        {
            if (FightHappening && !SomebodyLost)
            {

                if (EnemySideTurns.SideState.Health <= 0f)
                {
                    SomebodyLost = true;
                    StopAllCoroutines();
                    StartCoroutine(AnnounceWinner("Player"));
                }
                else if (PlayerSideTurns.SideState.Health <= 0f)
                {
                    SomebodyLost = true;
                    StopAllCoroutines();
                    StartCoroutine(AnnounceWinner("Enemy"));
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(AddItem<BulletItem>());
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(AddItem<HandItem>());
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                StartCoroutine(AddItem<KnifeItem>());
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                StartCoroutine(AddItem<GunpowderItem>());
            }
            // else if (Input.GetKeyDown(KeyCode.B))
            // {
            //     StartCoroutine(AddItem<ImprovedSkullItem>());
            // }

            if (!FightHappening)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TryStartFight();
                }
            }
        }
        public void TryStartFight()
        {
            SomebodyLost = false;
            FightHappening = true;
            StartCoroutine(PlayerSideTurns.UseAllSequence(PlayerSlots, EnemySideTurns));
            StartCoroutine(EnemySideTurns.UseAllSequence(EnemySlots, PlayerSideTurns));
        }
        public IEnumerator AddItem<T>() where T : CMSEntity
        {
            var basicItem = CMS.Get<T>();
            
            Debug.Log(basicItem.id);
            var interactiveObject = Instantiate(basicItem.Get<TagPfb>().Prefab);

            var state = new ItemState();

            var modifiables = basicItem.components.FindAll(x=> x is ModifiableComponentDefinition);

            state.ModifiableComponents = new();
            foreach (var modifiable in modifiables)
            {
                state.ModifiableComponents.Add(modifiable.DeepCopy() as ModifiableComponentDefinition);
            }
            state.Model = basicItem;
            state.View = interactiveObject;
            
            interactiveObject.SetState(state);
            
            PlayerSlots.AddItemToFreeSlot(interactiveObject);
            
            yield break;
        }
        
        // move it to turnsSideManager
        public IEnumerator NegateCooldown(InteractiveObject itemToBuff)
        {
            if (itemToBuff.ItemState.Is<TagUseDuration>(out var useDuration))
            {
                useDuration.Duration = 0f;
            }
            yield break;
        }
        public void CastForNewSlot(PointerEventData eventData, InteractiveObject draggedObject)
        {
            var ray = Camera.main.ScreenPointToRay(eventData.position);
            var raycastHit = Physics2D.RaycastAll(ray.origin, ray.direction, 100f, 1 << 3);
            foreach (var hit in raycastHit)
            {
                var itemHolder = hit.collider.gameObject.GetComponentInParent<ItemHolder>();
                itemHolder.Claim(draggedObject);
                break;
            }
        }
    }
}