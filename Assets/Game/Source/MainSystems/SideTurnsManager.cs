using System.Collections;
using DG.Tweening;
using Game.Source.Data;
using Game.Source.Data.Characters;
using Game.Source.Tags;
using UnityEngine;

namespace Game.Source
{
    public class SideTurnsManager
    {
        public SideTurnsManager OpposingSideTurns;
        public SideState SideState;
        public StatsView SideStatsView;
        public SlotArea Slots;
        public int CurrentItemIndex = 0;
        public GameObject SideCharacter;
        public SideTurnsManager(SideState sideState, StatsView sideStatsView, SlotArea slots, GameObject sideCharacter)
        {
            SideCharacter = sideCharacter;
            
            SideState = sideState.DeepCopy();
            SideStatsView = sideStatsView;
            
            sideStatsView.SetHealth(sideState.Health);
            sideStatsView.SetDamage(sideState.Damage);
            sideStatsView.SetDamageMulitplier(sideState.DamageMultiplier);
            sideStatsView.SetDefense(sideState.Defense);
            sideStatsView.SetDefenseMultiplier(sideState.DefenseMultiplier);

            Slots = slots;
        }
        public IEnumerator UseAllSequence(SlotArea slotArea, SideTurnsManager opponentSideTurns)
        {
            foreach (var slot in slotArea.FaceSlots)
            {
                slotArea.SlotPointer.TargetPosition = slot.CursorTarget.position;
                if (slot.InteractiveObject == null)
                {
                    CurrentItemIndex++;
                    continue;
                }
                var onUsed = G.main.Interactor.FindAll<IOnUse>();
                var item = slot.InteractiveObject;
                
                yield return item.StartTickingTimer();
                yield return new WaitForSeconds(item.ItemState.Get<TagUseDuration>().Duration);
                
                foreach (var onUse in onUsed)
                {
                    // perhaps move it out somewhere else and do a wait until?
                    // doesn't work well... okay i just found out about something crucial...
                    // yeah this actually doesn't work at all looool
                    yield return onUse.OnUse(item.ItemState, this, slotArea);
                }
                item.Exhaust();
                for (int i = CurrentItemIndex + 1; i < Slots.FaceSlots.Count; i++)
                {
                    var onNotUsed = G.main.Interactor.FindAll<IOnNotUsed>();
                    var notUsedItem = Slots.FaceSlots[i].InteractiveObject;
                    if (notUsedItem == null)
                        continue;
                    foreach (var notUsed in onNotUsed)
                    {
                        slotArea.SlotPointer.TargetPosition = slot.CursorTarget.position;
                        yield return notUsed.OnNotUsed(notUsedItem.ItemState, this, slotArea);
                    }
                }
                CurrentItemIndex++;
            }

            yield return RestoreSlots();
            yield return DealDamageToTheOpposingSide(opponentSideTurns);
        }
        public IEnumerator RestoreSlots()
        {
            CurrentItemIndex = 0;
            foreach (var slot in Slots.FaceSlots)
            {
                if (slot.InteractiveObject == null)
                    continue;
                var duration = slot.InteractiveObject.ItemState.Get<TagUseDuration>();
                duration.Duration = duration.BaseDuration;
                slot.InteractiveObject.UpdateTimeText();
                slot.InteractiveObject.Restore();
            }

            yield break;
        }
        public IEnumerator IncreaseDefMultiplier(float amount)
        {
            SideState.DefenseMultiplier += amount;
            SideStatsView.SetDefenseMultiplier(SideState.DefenseMultiplier);
            var defMulPos =  SideStatsView.DefenseMultiplier.transform.position;
            G.feel.CreateBasicPopup(defMulPos + Vector3.up * 1, "+ " + amount, Color.blue);
            yield break;
        }
        public IEnumerator TakeDamage(float damage)
        {
            SideCharacter.transform.DOShakePosition(0.4f, new Vector3(0.2f, 0.2f, 0));
            SideState.Health -= damage;
            SideStatsView.SetHealth(SideState.Health);
            // i dont know how to make this work better tbh
            var healthPos =  SideStatsView.Health.transform;
            G.feel.CreateBasicPopup(healthPos.position + Vector3.up * 1, "- " + damage, Color.orangeRed);
            yield break;
            // yield return OnTakeDamage or something like that
        }
        public IEnumerator IncreaseDamage(float amount, SideState sideState)
        {
            sideState.Damage += amount * sideState.DamageMultiplier;
            SideStatsView.SetDamage(SideState.Damage);
            var damagePos =  SideStatsView.Damage.transform.position;
            G.feel.CreateBasicPopup(damagePos + Vector3.up * 1, "+ " + amount * sideState.DamageMultiplier, Color.crimson);
            yield break;
            // yield return on damage dealt
        }
        public IEnumerator IncreaseDamageMultiplier(float amount)
        {
            SideState.DamageMultiplier += amount;
            SideStatsView.SetDamageMulitplier(SideState.DamageMultiplier);
            var multiplierPos =  SideStatsView.Damage.transform.position;
            G.feel.CreateBasicPopup(multiplierPos + Vector3.right * 1, "+ " + amount, Color.darkOrchid);
            yield break;
            // yield return on multiplier increased
        }
        public IEnumerator DealDamageToTheOpposingSide(SideTurnsManager opponent)
        {
            yield return opponent.TakeDamage(SideState.Damage);
            yield return UseAllSequence(Slots, opponent);
        }
    }
}