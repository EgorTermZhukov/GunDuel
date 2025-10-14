using System.Collections;
using Game.Source.Tags;

namespace Game.Source.Data.Characters
{
    public class SideState : EntityComponentDefinition
    {
        public float Health;
        public float Damage;
        public float DamageMultiplier;
        public float Defense;
        public float DefenseMultiplier;
    }
    public class TagSlotCount : EntityComponentDefinition
    {
        public int Count;
    }
    public class TagChargeDuration : ModifiableComponentDefinition
    {
        public float Duration;
    }
    public class BasicCharacter : CMSEntity
    {
        public BasicCharacter()
        {
            var state = Define<SideState>();
            state.Health = 100f;
            state.Damage = 1;
            state.DamageMultiplier = 1f;
            // not sure about defense for now, maybe scrap it? uhhh
            state.Defense = 0f;
            state.DefenseMultiplier = 1f;
            Define<TagSlotCount>().Count = 6;
            Define<TagChargeDuration>().Duration = 2f;
        }
    }
    // public class WeaponChargeInteraction : BaseInteraction, IOnWeaponStartsCharging
    // {
    //     
    // }
    //
    // public interface IOnWeaponStartsCharging
    // {
    //     public IEnumerator OnStartChargingWeapon(WeaponState weaponState, SideTurnsManager side);
    // }
}