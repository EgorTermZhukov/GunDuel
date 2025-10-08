using System;
using System.Collections.Generic;
using Game.Source;
using TMPro;
using UnityEngine;
using GameObject = UnityEngine.GameObject;

[Serializable]
public struct ItemSprite
{
    public Item Item;
    public Sprite Sprite;
}
public class SlotArea : MonoBehaviour
{
    [SerializeField] private GameObject _diceSlotPrefab;
    [SerializeField] private Transform Anchor;
    [SerializeField] public float Spacing;
    
    public List<InventorySlot> FaceSlots = new List<InventorySlot>();
    void Start()
    {
        
    }
    void Update()
    {
    }
    public InventorySlot AddSlot()
    {
        var faceSlotGO = Instantiate(_diceSlotPrefab);
        faceSlotGO.transform.parent = Anchor;
        
        // TODO maybe center them instead later on, or curve (no)
        // TODO when slot of the same type is added, add it downward (if this feature makes sense tho)
        faceSlotGO.transform.localPosition = Vector2.right * FaceSlots.Count * Spacing;
        var faceSlot = faceSlotGO.GetComponent<InventorySlot>();
        
        FaceSlots.Add(faceSlot);
        return faceSlot;
    }
}
