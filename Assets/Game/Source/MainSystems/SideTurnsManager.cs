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
        public CharacterView CharacterView;
        public SideTurnsManager(SideState sideState, StatsView sideStatsView, SlotArea slots, CharacterView characterView)
        {
            CharacterView = characterView;
            
            // dissect character stats to many different tags
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
            foreach (var slot in slotArea.InvSlots)
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
                 
                // ok, delegating this to the IOnUse yield return new WaitUntil(G.Ticker.CreatePr(item.ItemState.Get<TagUseDuration>().Duration));
                
                foreach (var onUse in onUsed)
                {
                    yield return onUse.OnUse(item.ItemState, this, slotArea);
                }
                item.Exhaust();
                CurrentItemIndex++;
            }

            yield return RestoreSlots();
            yield return DealDamageToTheOpposingSide(opponentSideTurns);
        }
        public IEnumerator RestoreSlots()
        {
            CurrentItemIndex = 0;
            foreach (var slot in Slots.InvSlots)
            {
                if (slot.InteractiveObject == null)
                    continue;
                
                var interactiveObject = slot.InteractiveObject;
                
                var level = interactiveObject.ItemState.Get<TagItemLevel>();
                var baseDuration = interactiveObject.ItemState.Model.Get<TagBaseUseDuration>();
                var duration = interactiveObject.ItemState.Get<TagUseDuration>();
                
                duration.Duration = baseDuration.Get(level);
                
                interactiveObject.UpdateTimeText();
                interactiveObject.Restore();
            }

            yield break;
        }
        public IEnumerator IncreaseDefMultiplier(float amount)
        {
            SideState.DefenseMultiplier += amount;
            SideStatsView.SetDefenseMultiplier(SideState.DefenseMultiplier);
            var defMulPos =  SideStatsView.DefenseMultiplier.transform.position;
            G.feel.CreateBasicPopup("+ " + amount, 0.5f, CharacterView.StatIncreaseTarget.transform.position, Color.cornflowerBlue, Icon.Defense);
            yield break;
        }
        public IEnumerator TakeDamage(float damage)
        {
            //CharacterView.transform.DOShakePosition(0.4f, new Vector3(0.2f, 0.2f, 0));
            SideState.Health -= damage;
            CharacterView.Flash();
            SideStatsView.SetHealth(SideState.Health);
            var healthPos =  SideStatsView.Health.transform;
            G.feel.CreateBasicPopup("- " + damage, 0.5f, CharacterView.StatDecreaseTarget.transform.position, Color.red, Icon.HealthDamaged);
            yield break;
        }
        public IEnumerator IncreaseDamage(float amount, SideState sideState)
        {
            var delta = amount * sideState.DamageMultiplier;
            sideState.Damage += delta;
            SideStatsView.SetDamage(SideState.Damage);
            var damagePos =  SideStatsView.Damage.transform.position;
            G.feel.CreateBasicPopup("+ " + delta, 0.5f, CharacterView.StatIncreaseTarget.transform.position, Color.lawnGreen, Icon.Damage);
            yield break;
            // yield return on damage dealt
        }
        public IEnumerator IncreaseDamageMultiplier(float amount)
        {
            SideState.DamageMultiplier += amount;
            SideStatsView.SetDamageMulitplier(SideState.DamageMultiplier);
            var multiplierPos =  SideStatsView.Damage.transform.position;
            G.feel.CreateBasicPopup("+x" + amount, 0.5f, CharacterView.StatIncreaseTarget.transform.position, Color.lawnGreen, Icon.None);
            yield break;
        }
        public IEnumerator DealDamageToTheOpposingSide(SideTurnsManager opponent)
        {
            CharacterView.Weapon.rotation = Quaternion.identity;
            CharacterView.Weapon.DORotate(new(0, 0, 360f), 1f, RotateMode.FastBeyond360).SetRelative();
            yield return new WaitUntil(G.Ticker.CreatePr(1f));
            
            CharacterView.Weapon.DOShakePosition(0.2f, new Vector3(0.2f, 0.2f, 0));
            
            G.ParticleController.Spawn(opponent.CharacterView.transform.position, ParticleType.Shoot);
            
            yield return opponent.TakeDamage(SideState.Damage);
            yield return UseAllSequence(Slots, opponent);
        }
    }
}