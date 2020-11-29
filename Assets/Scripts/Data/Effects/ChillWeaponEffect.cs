using Pixeye.Actors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ActorsECS.Data.Effects
{
  [CreateAssetMenu(fileName = "Chill Effect", menuName = "Data/Create/Effects/Chill Effect", order = 0)]
  public class ChillWeaponEffect : WeaponItem.WeaponAttackEffect
  {
    public float PercentageChance;
    public int Damage;
    public float Time;
    
    public override void OnAttack(ent target, ent user, ref WeaponItem.AttackData attackData)
    {
      if (!(Random.value < (PercentageChance / 100.0f))) return;

      attackData.AddDamage(StatSystem.DamageType.Cold, Damage);
        
      Debug.Log("Chill effect is applied");
    }
  }
}