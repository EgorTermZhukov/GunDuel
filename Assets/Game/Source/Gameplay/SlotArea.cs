using System;
using System.Collections;
using System.Collections.Generic;
using Game.Source;
using Game.Source.Data;
using Game.Source.Tags;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using GameObject = UnityEngine.GameObject;

[Serializable]
public struct ItemSprite
{
    public Item Item;
    public Sprite Sprite;
}
public class SlotArea : MonoBehaviour
{
    [SerializeField] public MoveableBase SlotPointer;
    [SerializeField] private GameObject _diceSlotPrefab;
    [SerializeField] private Transform Anchor;
    [SerializeField] public float Spacing;

    public int CurrentItemIndex = 0;
    
    [FormerlySerializedAs("FaceSlots")] public List<InventorySlot> InvSlots = new List<InventorySlot>();
    void Start()
    {
        
    }
    void Update()
    {
    }
    public void TryMerge(ItemHolder itemHolder, InteractiveObject interactiveObject)
    {
        var itemLevel = itemHolder.InteractiveObject.ItemState.Get<TagItemLevel>();
        if (itemLevel.Level >= itemLevel.MaxLevel)
            return;
        if (itemHolder.InteractiveObject.ItemState.Model != interactiveObject.ItemState.Model)
            return;
        if (itemHolder.InteractiveObject.ItemState.Get<TagItemLevel>().Level !=
            interactiveObject.ItemState.Get<TagItemLevel>().Level)
            return;
        
        itemLevel.Level++;
        
        if(interactiveObject.ItemHolder != null)
            interactiveObject.ItemHolder.Release();
        
        StartCoroutine(MergeAnimationRoutine(itemHolder.InteractiveObject, interactiveObject));
    }

    public IEnumerator MergeAnimationRoutine(InteractiveObject toImprove, InteractiveObject toDestroy)
    {
        toDestroy.Moveable.TargetPosition = toImprove.transform.position;
        toDestroy.IsLocked = true;

        yield return new WaitUntil(G.Ticker.CreatePr(0.1f));
        // TODO play particles
        Destroy(toDestroy.gameObject);
        toImprove.UpdateLevel();
    }
    public InventorySlot AddSlot()
    {
        var faceSlotGO = Instantiate(_diceSlotPrefab);
        faceSlotGO.transform.parent = Anchor;
        
        // TODO maybe center them instead later on, or curve (no)
        // TODO when slot of the same type is added, add it downward (if this feature makes sense tho)
        faceSlotGO.transform.localPosition = Vector2.right * InvSlots.Count * Spacing;
        var faceSlot = faceSlotGO.GetComponent<InventorySlot>();

        faceSlot.OnAttemptToOccupy += TryMerge;
        
        InvSlots.Add(faceSlot);
        
        return faceSlot;
    }
    public bool AddItemToFreeSlot(InteractiveObject interactiveObject)
    {
        var freeSlot = InvSlots.Find(x => x.InteractiveObject == null);
        if (freeSlot == null)
            return false;
        freeSlot.Claim(interactiveObject);
        return true;
    }
}
