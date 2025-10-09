using System.Collections;
using Game.Source.Data.Characters;
using UnityEngine;

namespace Game.Source
{
    public class SideManager
    {
        public SideState SideState;
        public SideManager(SideState sideState)
        {
            SideState = sideState.DeepCopy();
        }
        public IEnumerator IncreaseDamage(float amount, SideState sideState)
        {
            Debug.Log("Increasing damage of the side! PreviousDamage: " + SideState.Damage);
            sideState.Damage += amount;
            Debug.Log("New damage: " + SideState.Damage);
            
            yield break;
        }
    }
}