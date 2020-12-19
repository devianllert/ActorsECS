using System;
using System.Linq;
using ActorsECS.Core.Modules.Common;
using ActorsECS.Data.Systems;
using Pixeye.Actors;

namespace ActorsECS.Data.Items
{
  /// <summary>
  ///   This class will store damage done to a target CharacterData by a source CharacterData. The function to add
  ///   damage will take care of applied all the strength/boost of the source and remove defense/resistance of the
  ///   target.
  ///   The source can be null when its done by an non CharacterData source (elemental effect, environment etc.)
  /// </summary>
  public class AttackData
  {
    public readonly int[] _damages = new int[Enum.GetValues(typeof(StatSystem.DamageType)).Length];
    private readonly ent _source;

    private readonly ent _target;

    /// <summary>
    ///   Build a new AttackData. All AttackData need a target, but source is optional. If source is null, the
    ///   damage is assume to be from a non CharacterData source (elemental effect, environment) and no boost will
    ///   be applied to damage (target defense is still taken in account).
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public AttackData(ent target, ent source = default)
    {
      _target = target;
      _source = source;
    }

    public ent Target => _target;
    public ent Source => _source;

    /// <summary>
    ///   Add an amount of damage given in the given type. The source (if non null, see class documentation for
    ///   info) boost will be applied and the target defense will be removed from that amount.
    /// </summary>
    /// <param name="damageType">The type of damage</param>
    /// <param name="amount">The amount of damage</param>
    /// <returns>The amount of *effective* damage</returns>
    public int AddDamage(StatSystem.DamageType damageType, int amount)
    {
      ref var targetStatSystem = ref _target.ComponentStats().statSystem;
      ref var sourceStatSystem = ref _source.ComponentStats().statSystem;

      var addedAmount = amount;

      addedAmount = damageType == StatSystem.DamageType.Physical
        ? Defense.CalculateDamageAfterDR(targetStatSystem.stats.defense, addedAmount)
        : Defense.CalculateElementalDamage(targetStatSystem.stats.elementalProtection[(int) damageType], addedAmount);

      // //we then add boost per damage type. Not this is called elementalBoost, but physical can also be boosted
      // if (_source.exist)
      //   addedAmount += addedAmount * Mathf.FloorToInt(sourceStatSystem.stats.elementalBoosts[(int)damageType] / 100.0f);

      _damages[(int) damageType] += addedAmount;

      return addedAmount;
    }

    /// <summary>
    ///   Return the current amount of damage of the given type stored in that AttackData. This is the *effective*
    ///   amount of damage, boost and defense have already been applied.
    /// </summary>
    /// <param name="damageType">The type of damage</param>
    /// <returns>How much damage of that type is stored in that AttackData</returns>
    public int GetDamage(StatSystem.DamageType damageType)
    {
      return _damages[(int) damageType];
    }

    /// <summary>
    ///   Return the total amount of damage across all type stored in that AttackData. This is *effective* damage,
    ///   that mean all boost/defense was already applied.
    /// </summary>
    /// <returns>The total amount of damage across all type in that Attack Data</returns>
    public int GetFullDamage()
    {
      return _damages.Sum();
    }
  }
}