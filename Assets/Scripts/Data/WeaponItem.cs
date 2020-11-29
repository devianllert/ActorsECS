using System.Collections.Generic;
using System.Linq;
using ActorsECS.Modules.Common;
using ActorsECS.Modules.Loot.Components;
using ActorsECS.VFX;
using Pixeye.Actors;
using UnityEngine;
using Random = Pixeye.Actors.Random;

namespace ActorsECS.Data
{
  [CreateAssetMenu(fileName = "Weapon", menuName = "Data/Create/Weapon", order = 0)]
  public class WeaponItem : EquipmentItem
  {
    [Header("Bullet Type")]
    public int bulletType = 0;

        
    [System.Serializable]
    public struct Stat
    {
      public float rateOfFire;
      public float range;
      public float speed;
      public int minimumDamage;
      public int maximumDamage;
      public int ammo;
      public float reloadTime;
    }
    
    [Header("Stats")]
    public Stat stats = new Stat
    {
      speed = 1.0f,
      maximumDamage = 1,
      minimumDamage = 1,
      range = 1,
      ammo = 1,
      reloadTime = 1,
      rateOfFire = 1,
    };
    
    [Header("Sounds")]
    public AudioClip[] hitSounds;
    public AudioClip[] shootSounds;

    [Space]
    [Header("Projectile Data")]
    public GameObject projectilePrefab;
    public VFXType HitVFX;

    [Space]
    [Header("Hit Effects")]
    public List<WeaponAttackEffect> attackEffects;

    public override void Pickup(ent character, ent loot)
    {
      ref var cWeapon = ref character.Get<ComponentWeapon>();
      ref var lootItem = ref loot.ComponentLootData().item;
      ref var cLootWeapon = ref loot.Get<ComponentWeapon>();
      var weaponController = character.GetMono<CharacterWeaponController>();
      
      weaponController.SetupWeaponModel((WeaponItem) lootItem);
      
      if (cWeapon.equippedWeapon) DropWeapon(character);
      
      ref var equippedWeapon = ref cWeapon.equippedWeapon;
      
      var lootedWeapon = (WeaponItem) lootItem;
      
      cWeapon.currentAmmo = cLootWeapon.equippedWeapon ? cLootWeapon.currentAmmo : lootedWeapon.stats.ammo;
      equippedWeapon = lootedWeapon;
      
    }

    public void DropWeapon(ent character)
    {
      var Layer = character.layer;

      ref var cWeapon = ref character.ComponentWeapon();
      var weapon = cWeapon.equippedWeapon;
      var weaponAmmo = cWeapon.currentAmmo;

      cWeapon.equippedWeapon = null;
      cWeapon.currentAmmo = 0;

      var forward = character.transform.forward;
      
      var prevLoot = Layer.Actor.Create("Prefabs/Loot", character.transform.position + forward);

      Layer.Obj.Create(weapon.worldObjectPrefab, prevLoot.transform).gameObject.AddComponent<Outline>();
      
      ref var newLoot = ref prevLoot.entity.Get<ComponentLootData>();
      ref var cPrevWeapon = ref prevLoot.entity.Get<ComponentWeapon>();
      
      cPrevWeapon.currentAmmo = weaponAmmo;
      cPrevWeapon.equippedWeapon = weapon;
      
      newLoot.item = weapon;
      
      prevLoot.GetComponent<Rigidbody>().AddForce((forward + Vector3.up) * 2f, ForceMode.Impulse);
    }
    
    public override string GetDescription()
    {
      string desc = base.GetDescription();

      int minimumDPS = Mathf.RoundToInt(stats.minimumDamage / stats.speed);
      int maximumDPS = Mathf.RoundToInt(stats.maximumDamage / stats.speed);

      desc += "\n";
      desc += $"Damage: {stats.minimumDamage} - {stats.maximumDamage}\n";
      desc += $"DPS: {minimumDPS} - {maximumDPS}\n";
      desc += $"Attack Speed : {stats.speed}s\n";
      desc += $"Range : {stats.range}m\n";

      return desc;
    }
    
    public class AttackData
    {
      public ent Target => _target;
      public ent Source => _source;
            
      private readonly ent _target;
      private readonly ent _source;

      public readonly int[] _damages = new int[System.Enum.GetValues(typeof(StatSystem.DamageType)).Length];
      
      /// <summary>
      /// Build a new AttackData. All AttackData need a target, but source is optional. If source is null, the
      /// damage is assume to be from a non CharacterData source (elemental effect, environment) and no boost will
      /// be applied to damage (target defense is still taken in account).
      /// </summary>
      /// <param name="target"></param>
      /// <param name="source"></param>
      public AttackData(ent target, ent source = default)
      {
        _target = target;
        _source = source;
      }
      
      /// <summary>
      /// Add an amount of damage given in the given type. The source (if non null, see class documentation for
      /// info) boost will be applied and the target defense will be removed from that amount.
      /// </summary>
      /// <param name="damageType">The type of damage</param>
      /// <param name="amount">The amount of damage</param>
      /// <returns></returns>
      public int AddDamage(StatSystem.DamageType damageType, int amount)
      {
        ref var targetStatSystem = ref _target.ComponentStats().StatSystem;
        ref var sourceStatSystem = ref _source.ComponentStats().StatSystem;

        var addedAmount = amount;

        addedAmount = damageType == StatSystem.DamageType.Physical
          ? Defense.CalculateDamageAfterDR(targetStatSystem.stats.defense, addedAmount)
          : Defense.CalculateElementalDamage(targetStatSystem.stats.elementalProtection[(int)damageType], addedAmount);
            
        // //we then add boost per damage type. Not this is called elementalBoost, but physical can also be boosted
        // if (_source.exist)
        //   addedAmount += addedAmount * Mathf.FloorToInt(sourceStatSystem.stats.elementalBoosts[(int)damageType] / 100.0f);

        _damages[(int)damageType] += addedAmount;

        return addedAmount;
      }
      
      /// <summary>
      /// Return the current amount of damage of the given type stored in that AttackData. This is the *effective*
      /// amount of damage, boost and defense have already been applied.
      /// </summary>
      /// <param name="damageType">The type of damage</param>
      /// <returns>How much damage of that type is stored in that AttackData</returns>
      public int GetDamage(StatSystem.DamageType damageType)
      {
        return _damages[(int)damageType];
      }
      
      /// <summary>
      /// Return the total amount of damage across all type stored in that AttackData. This is *effective* damage,
      /// that mean all boost/defense was already applied.
      /// </summary>
      /// <returns>The total amount of damage across all type in that Attack Data</returns>
      public int GetFullDamage() => _damages.Sum();
    }
    
    
    public void Attack(ent attacker, ent target)
    {
      var attackData = new AttackData(target, attacker);

      var damage = Random.Range(stats.minimumDamage, stats.maximumDamage + 1);
      
      attackData.AddDamage(StatSystem.DamageType.Physical, damage);
      
      foreach(var wae in attackEffects)
        wae.OnAttack(target, attacker, ref attackData);
       
      target.ComponentStats().StatSystem.Damage(attackData);
        
      foreach(var wae in attackEffects)
        wae.OnPostAttack(target, attacker, attackData);
    }
    
    /// <summary>
    /// Base class of all effect you can add on a weapon to specialize it. See documentation on How to write a new
    /// Weapon Effect.
    /// </summary>
    public abstract class WeaponAttackEffect : ScriptableObject
    {
      public string description;
        
      //return the amount of physical damage. If no change, just return physicalDamage passed as parameter
      public virtual void OnAttack(ent target, ent user, ref AttackData data) { }
        
      //called after all weapon effect where applied, allow to react to the total amount of damage applied
      public virtual void OnPostAttack(ent target, ent user, AttackData data) { }

      public virtual string GetDescription()
      {
        return description;
      }
    }
  }
}
