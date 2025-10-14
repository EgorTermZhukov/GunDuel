using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Source
{
    public class StatsView : MonoBehaviour
    {
        [SerializeField] public TMP_Text Health;
        [SerializeField] public TMP_Text Damage;
        [SerializeField] public TMP_Text DamageMultiplier;
        [SerializeField] public TMP_Text Defense;
        [SerializeField] public TMP_Text DefenseMultiplier;

        public void SetHealth(float health)
        {
            Health.transform.DOKill();
            Health.transform.localScale = Vector3.one;
            Health.text = health.ToString();
            Health.transform.DOPunchScale(new(0.2f, 0.2f, 0.2f), 0.3f);
        }
        public void SetDamage(float damage)
        {
            Damage.transform.DOKill();
            Damage.transform.localScale = Vector3.one;
            Damage.text = damage.ToString();
            Damage.transform.DOPunchScale(new(0.2f, 0.2f, 0.2f), 0.3f);
        }
        public void SetDamageMulitplier(float damageMul)
        {
            DamageMultiplier.transform.DOKill();
            DamageMultiplier.transform.localScale = Vector3.one;
            DamageMultiplier.text = damageMul.ToString();
            DamageMultiplier.transform.DOPunchScale(new(0.2f, 0.2f, 0.2f), 0.3f);
        }
        public void SetDefense(float defense)
        {
            Defense.transform.DOKill();
            DefenseMultiplier.transform.localScale = Vector3.one;
            Defense.text = defense.ToString();
            Defense.transform.DOPunchScale(new(0.2f, 0.2f, 0.2f), 0.3f);
        }
        public void SetDefenseMultiplier(float defenseMul)
        {
            DefenseMultiplier.transform.DOKill();
            DefenseMultiplier.transform.localScale = Vector3.one;
            DefenseMultiplier.text = defenseMul.ToString();
            DefenseMultiplier.transform.DOPunchScale(new(0.2f, 0.2f, 0.2f), 0.3f);
        }
    }
}