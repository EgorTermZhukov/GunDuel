using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Source
{
    public class DiceRoller
    {
        public int MinValue = 1;
        public int MaxValue = 6;
        
        public float GetRandomWeight(float maxWeight)
        {
            return Random.Range(0f, maxWeight);
        }
        public void Roll(SlotArea slotArea)
        {
            var face = slotArea.RollSlot(this);
            // shouldn't be possible
            if (face == null)
                throw new NullReferenceException();

            Debug.Log("Rolled " + face.Value);
            
            if (face.InteractiveObject == null)
                return;
            
            ExecuteItem(face, slotArea);
            face.Activate();
        }
        // It's nice to have an executor somewhere outside the ItemView but i dunno how to do it at the moment, maybe that executor should be global and static
        public void ExecuteItem(FaceSlot face, SlotArea slotArea)
        {
            var item = face.InteractiveObject.ItemData.ItemType;
     
            switch (item)
            {
                case Item.Gun:
                    Debug.Log("Gun");
                    face.InteractiveObject.ItemData.BaseProbabilityWeight = 1f;
                    var exhaustedBullets = slotArea.FaceSlots.FindAll(x => x.InteractiveObject?.ItemData.ItemType == Item.Bullet 
                                                                           && x.InteractiveObject?.ItemData.BaseProbabilityWeight <= 0f);
                    foreach (var bulletFace in exhaustedBullets)
                    {
                        bulletFace.InteractiveObject.ItemData.BaseProbabilityWeight = 1f;
                        bulletFace.InteractiveObject.Restore();
                    }
                    slotArea.RecalculateProbabilities();
                    break;
                case Item.Bullet:
                    Debug.Log("Bullet");
                    face.InteractiveObject.ItemData.BaseProbabilityWeight = 0f;
                    face.InteractiveObject.Exhaust();
                    var gun = slotArea.FaceSlots.Find(x=> x.InteractiveObject?.ItemData.ItemType == Item.Gun).InteractiveObject;
                    if (gun != null)
                        gun.ItemData.BaseProbabilityWeight += 2f;
                    slotArea.RecalculateProbabilities();
                    break;
            }
        }
    }
}