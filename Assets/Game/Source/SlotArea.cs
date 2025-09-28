using System;
using System.Collections.Generic;
using Game.Source;
using TMPro;
using UnityEngine;

[Serializable]
public struct SlotItemSprite
{
    public SlotItem SlotItem;
    public Sprite Sprite;
}
public class SlotArea : MonoBehaviour
{
    [SerializeField] private GameObject _diceSlotPrefab;
    [SerializeField] private Transform Anchor;
    [SerializeField] public float Spacing;
    [SerializeField] private List<SlotItemSprite> _slotItemSprites = new List<SlotItemSprite>();
    
    private List<DiceSlot> _slots = new List<DiceSlot>();
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public DiceSlot AddSlot(int diceValue, SlotItem slotItem)
    {
        var diceSlotGO = Instantiate(_diceSlotPrefab);
        diceSlotGO.transform.parent = Anchor;
        
        // TODO maybe center them instead later on, or curve (no)
        diceSlotGO.transform.localPosition = Vector2.right * _slots.Count * Spacing;
        var diceSlot = diceSlotGO.GetComponent<DiceSlot>();
        diceSlot.Initialize(diceValue, slotItem, _slotItemSprites);
        _slots.Add(diceSlot);
        
        return diceSlot;
    }

    public List<DiceSlot> GetSlotsWithCorrespondingValue(int diceValue)
    {
        return _slots.FindAll(x => x.Value == diceValue);
    }
}
