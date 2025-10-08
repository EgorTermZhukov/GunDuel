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
    
    public List<FaceSlot> FaceSlots = new List<FaceSlot>();
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (var slot in FaceSlots)
            {
                Debug.Log("Value: " + slot.Value + "Item: " + slot.InteractiveObject.ItemData.ItemType);
            }
        }
    }
    public FaceSlot AddSlot(int diceValue, float baseWeight)
    {
        var faceSlotGO = Instantiate(_diceSlotPrefab);
        faceSlotGO.transform.parent = Anchor;
        
        // TODO maybe center them instead later on, or curve (no)
        // TODO when slot of the same type is added, add it downward (if this feature makes sense tho)
        faceSlotGO.transform.localPosition = Vector2.right * FaceSlots.Count * Spacing;
        var faceSlot = faceSlotGO.GetComponent<FaceSlot>();
        faceSlot.Value = diceValue;
        faceSlot.Weight = baseWeight;
        
        faceSlot.OnItemClaimed += RecalculateProbabilities;
        faceSlot.OnItemReleased += RecalculateProbabilities;
        
        FaceSlots.Add(faceSlot);
        return faceSlot;
    }
    public List<FaceSlot> GetSlotsWithCorrespondingValue(int diceValue)
    {
        return FaceSlots.FindAll(x => x.Value == diceValue);
    }

    public FaceSlot RollSlot(DiceRoller roller)
    {
        float effectiveSumWeight = 0f;
        foreach (var face in FaceSlots)
        {
            var faceWeight = face.Weight;
            var itemWeight = face.InteractiveObject != null ? face.InteractiveObject.ItemData.BaseProbabilityWeight : 1f;
            effectiveSumWeight += faceWeight * itemWeight;
        }
        var rolledWeight = roller.GetRandomWeight(effectiveSumWeight);
        Debug.Log("Rolled weight: " + rolledWeight);
        
        foreach (var face in FaceSlots)
        {
            var faceWeight = face.Weight;
            var itemWeight = face.InteractiveObject != null ? face.InteractiveObject.ItemData.BaseProbabilityWeight : 1f;
            var effectiveWeight = faceWeight * itemWeight;
            rolledWeight -= effectiveWeight;
            if (rolledWeight <= 0f)
                return face;
        }
        return null;
    }
    public void RecalculateProbabilities()
    {
        //TODO resolve divide by zero
        float sumWeights = 0f;

        foreach (var face in FaceSlots)
        {
            var faceWeight = face.Weight;
            var itemWeight = face.InteractiveObject != null ? face.InteractiveObject.ItemData.BaseProbabilityWeight : 1f;
            sumWeights += faceWeight * itemWeight;
        }
        Debug.Log("Updated probability, sum of weights: " + sumWeights);
        // now set the text for every face
        foreach (var face in FaceSlots)
        {
            var faceWeight = face.Weight;
            var itemWeight = face.InteractiveObject != null ? face.InteractiveObject.ItemData.BaseProbabilityWeight : 1f;
            var combinedWeight = faceWeight * itemWeight;
            var probability = combinedWeight / sumWeights;
            face.SetProbabilityText(probability * 100);
        }
    }
}
