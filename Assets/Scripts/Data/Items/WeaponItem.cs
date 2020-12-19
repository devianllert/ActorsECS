using System;
using System.Collections.Generic;
using ActorsECS.Data.Projectiles;
using ActorsECS.Data.Systems;
using ActorsECS.Modules.Common;
using ActorsECS.Modules.Loot.Components;
using Pixeye.Actors;
using UnityEngine;
using Random = Pixeye.Actors.Random;

namespace ActorsECS.Data.Items
{
  /// <summary>
  ///   Special case of EquipmentItem for weapon, as they have a whole attack system in addition. Like Equipment they
  ///   can have minimum stats and equipped effect, but also have a list of WeaponAttackEffect that will have their
  ///   OnAttack function called during a hit, and their OnPostAttack function called after all OnAttack of all effects
  ///   are called.
  /// </summary>
  [CreateAssetMenu(fileName = "Weapon", menuName = "Data/Create/Weapon", order = 0)]
  public class WeaponItem : EquipmentItem
  {
    [Header("Stats")]
    public Stat stats = new Stat
    {
      speed = 1.0f,
      maximumDamage = 1,
      minimumDamage = 1,
      range = 1,
      ammo = 1,
      reloadTime = 1,
      rateOfFire = 1
    };

    [Header("Animations")]
    public AnimationClip handGripAnimation;

    [Header("Sounds")]
    public AudioClip[] hitSounds;
    public AudioClip[] shootSounds;

    [Space]
    [Header("Projectile")]
    public ProjectileBehaviour projectileBehaviour;

    [Space]
    [Header("Hit Effects")]
    public List<WeaponAttackEffect> attackEffects;

    public override void Pickup(ent character, ent loot)
    {
      base.Pickup(character, loot);
      
      var hasAmmo = loot.Has<ComponentWeapon>();
      
      ref var cWeapon = ref character.Get<ComponentWeapon>();
      ref var cEquipment = ref character.ComponentEquipment();
      ref var lootItem = ref loot.ComponentLootData().item;
      ref var cLootWeapon = ref loot.Get<ComponentWeapon>();
      
      if (cEquipment.equipmentSystem.Weapon) Drop(character);

      cEquipment.equipmentSystem.Equip(lootItem as EquipmentItem);
      
      cWeapon.currentAmmo = hasAmmo ? cLootWeapon.currentAmmo : (lootItem as WeaponItem).stats.ammo;
    }

    public override void Drop(ent character)
    {
      var Layer = character.layer;

      ref var cWeapon = ref character.ComponentWeapon();
      ref var cEquipment = ref character.ComponentEquipment();
      var droppedWeapon = cEquipment.equipmentSystem.Weapon;

      cEquipment.equipmentSystem.Unequip(EquipmentSlot.Weapon);
      
      var weaponAmmo = cWeapon.currentAmmo;

      cWeapon.currentAmmo = 0;

      var forward = character.transform.forward;

      var prevLoot = Layer.Actor.Create("Prefabs/Loot", character.transform.position + forward);

      Layer.Obj.Create(droppedWeapon.worldObjectPrefab, prevLoot.transform).gameObject.AddComponent<Outline>();

      ref var newLoot = ref prevLoot.entity.Get<ComponentLootData>();
      ref var cPrevWeapon = ref prevLoot.entity.Get<ComponentWeapon>();

      cPrevWeapon.currentAmmo = weaponAmmo;

      newLoot.item = droppedWeapon;
      
      prevLoot.GetComponent<Rigidbody>().AddForce((forward + Vector3.up) * 2f, ForceMode.Impulse);
    }

    public override string GetDescription()
    {
      var desc = base.GetDescription();

      var minimumDPS = Mathf.RoundToInt(stats.minimumDamage / stats.speed);
      var maximumDPS = Mathf.RoundToInt(stats.maximumDamage / stats.speed);

      desc += "\n";
      desc += $"Damage: {stats.minimumDamage} - {stats.maximumDamage}\n";
      desc += $"DPS: {minimumDPS} - {maximumDPS}\n";
      desc += $"Attack Speed : {stats.speed}s\n";
      desc += $"Range : {stats.range}m\n";

      return desc;
    }


    public void Attack(ent attacker, ent target)
    {
      var attackData = new AttackData(target, attacker);

      var damage = Random.Range(stats.minimumDamage, stats.maximumDamage + 1);

      attackData.AddDamage(StatSystem.DamageType.Physical, damage);

      foreach (var wae in attackEffects)
        wae.OnAttack(target, attacker, ref attackData);

      target.ComponentStats().statSystem.Damage(attackData);

      foreach (var wae in attackEffects)
        wae.OnPostAttack(target, attacker, attackData);
    }


    [Serializable]
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

    /// <summary>
    ///   Base class of all effect you can add on a weapon to specialize it. See documentation on How to write a new
    ///   Weapon Effect.
    /// </summary>
    public abstract class WeaponAttackEffect : ScriptableObject
    {
      public string description;

      //return the amount of physical damage. If no change, just return physicalDamage passed as parameter
      public virtual void OnAttack(ent target, ent user, ref AttackData data)
      {
      }

      //called after all weapon effect where applied, allow to react to the total amount of damage applied
      public virtual void OnPostAttack(ent target, ent user, AttackData data)
      {
      }

      public virtual string GetDescription()
      {
        return description;
      }
    }
  }
}