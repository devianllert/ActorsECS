using System;
using System.Collections.Generic;
using ActorsECS.Data.Effects;
using ActorsECS.Data.Items;
using ActorsECS.Modules;
using Pixeye.Actors;
using TMPro;
using UnityEngine;
using Random = Pixeye.Actors.Random;
using Time = UnityEngine.Time;

namespace ActorsECS.Data.Systems
{
  /// <summary>
  ///   Handles the stats of a CharacterData. It stores the health and strength/agility/defense stats.
  ///   This class contains various functions for interacting with stats, by adding stat modifications, elemental
  ///   effects or damage.
  /// </summary>
  [Serializable]
  public class StatSystem
  {
    /// <summary>
    ///   The type of damage that exist, each will have their own boost/protection in stats, only the main, Physical,
    ///   is influence by strength and defense stats.
    /// </summary>
    public enum DamageType
    {
      Physical,
      Fire,
      Cold,
      Lightning,

      Chaos
      //ADD YOUR CUSTOM TYPE AFTER
    }

    public Stats baseStats;

    private List<StatModifier> _modifiersStack = new List<StatModifier>();

    private ent _owner;

    private Color[] DamageTypeColor =
    {
      Color.white,
      Color.red,
      Color.blue,
      Color.cyan,
      Color.green
    };

    public Stats stats { get; set; } = new Stats();

    public int CurrentHealth { get; private set; }
    public List<BaseElementalEffect> ElementalEffects { get; } = new List<BaseElementalEffect>();

    public List<TimedStatModifier> TimedModifierStack { get; } = new List<TimedStatModifier>();

    public void Init(ent owner)
    {
      stats.Copy(baseStats);
      CurrentHealth = stats.health;
      _owner = owner;
    }

    /// <summary>
    ///   Add a modifier to the end of the stack. This will recompute the Stats so it now include the new modifier.
    /// </summary>
    /// <param name="modifier"></param>
    public void AddModifier(StatModifier modifier)
    {
      _modifiersStack.Add(modifier);
      UpdateFinalStats();
    }

    /// <summary>
    ///   Remove a modifier from the stack. This modifier need to already be on the stack. e.g. used by the equipment
    ///   effect that store the modifier they add on equip and remove it when unequipped.
    /// </summary>
    /// <param name="modifier"></param>
    public void RemoveModifier(StatModifier modifier)
    {
      _modifiersStack.Remove(modifier);
      UpdateFinalStats();
    }

    /// <summary>
    ///   Add a Timed modifier. Timed modifier does not stack and instead re-adding the same type of modifier will just
    ///   reset the already existing one timer to the given duration. That the use of the id parameter : it need to be
    ///   shared by all timed effect that are the "same type". i.e. an effect that add strength can use "StrengthTimed"
    ///   as id, so if 2 object try to add that effect, they won't stack but instead just refresh the timer.
    /// </summary>
    /// <param name="modifier">A StatModifier container the wanted modification</param>
    /// <param name="duration">The time during which that modification will be active.</param>
    /// <param name="id">
    ///   A name that identify that type of modification. Adding a timed modification with an id that already
    ///   exist reset the timer instead of adding a new one to the stack
    /// </param>
    /// <param name="sprite">The sprite used to display the time modification above the player UI</param>
    public void AddTimedModifier(StatModifier modifier, float duration, string id, Sprite sprite)
    {
      var found = false;
      var index = TimedModifierStack.Count;
      for (var i = 0; i < TimedModifierStack.Count; ++i)
        if (TimedModifierStack[i].id == id)
        {
          found = true;
          index = i;
        }

      if (!found) TimedModifierStack.Add(new TimedStatModifier {id = id});

      TimedModifierStack[index].effectSprite = sprite;
      TimedModifierStack[index].duration = duration;
      TimedModifierStack[index].modifier = modifier;
      TimedModifierStack[index].Reset();

      UpdateFinalStats();
    }

    /// <summary>
    ///   Add an elemental effect to the StatSystem. Elemental Effect does not stack, adding the same type (the Equals
    ///   return true) will instead replace the old one with the new one.
    /// </summary>
    /// <param name="effect"></param>
    public void AddElementalEffect(BaseElementalEffect effect)
    {
      effect.Applied(_owner);

      var replaced = false;
      for (var i = 0; i < ElementalEffects.Count; ++i)
        if (effect.Equals(ElementalEffects[i]))
        {
          replaced = true;
          ElementalEffects[i].Removed();
          ElementalEffects[i] = effect;
        }

      if (!replaced)
        ElementalEffects.Add(effect);
    }

    public void Death()
    {
      foreach (var e in ElementalEffects)
        e.Removed();

      ElementalEffects.Clear();
      TimedModifierStack.Clear();

      UpdateFinalStats();
    }

    public void Tick(float deltaTime)
    {
      var needUpdate = false;

      for (var i = 0; i < TimedModifierStack.Count; ++i)
        //permanent modifier will have a timer == -1.0f, so jump over them
        if (TimedModifierStack[i].timer > 0.0f)
        {
          TimedModifierStack[i].timer -= deltaTime;
          if (TimedModifierStack[i].timer <= 0.0f)
          {
            //modifier finished, so we remove it from the stack
            TimedModifierStack.RemoveAt(i);
            i--;
            needUpdate = true;
          }
        }

      if (needUpdate)
        UpdateFinalStats();

      for (var i = 0; i < ElementalEffects.Count; ++i)
      {
        var effect = ElementalEffects[i];
        effect.Tick(this);

        if (effect.Done)
        {
          ElementalEffects[i].Removed();
          ElementalEffects.RemoveAt(i);
          i--;
        }
      }
    }

    /// <summary>
    ///   Change the health by the given amount : negative amount damage, positive amount heal. The function will
    ///   take care of clamping the value in the range [0...MaxHealth]
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHealth(int amount)
    {
      CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, stats.health);
    }

    private void UpdateFinalStats()
    {
      var maxHealthChange = false;
      var previousHealth = stats.health;

      stats.Copy(baseStats);

      foreach (var modifier in _modifiersStack)
      {
        if (modifier.stats.health != 0)
          maxHealthChange = true;

        stats.Modify(modifier);
      }

      foreach (var timedModifier in TimedModifierStack)
      {
        if (timedModifier.modifier.stats.health != 0)
          maxHealthChange = true;

        stats.Modify(timedModifier.modifier);
      }

      //if we change the max health we update the current health to it's new value
      if (maxHealthChange)
      {
        var percentage = CurrentHealth / (float) previousHealth;
        CurrentHealth = Mathf.RoundToInt(percentage * stats.health);
      }
    }

    /// <summary>
    ///   Will damage (change negatively health) of the amount of damage stored in the attackData. If the damage are
    ///   negative, this heal instead.
    ///   This will also notify the DamageUI so a damage number is displayed.
    /// </summary>
    /// <param name="attackData"></param>
    public void Damage(AttackData attackData)
    {
      var totalDamage = attackData.GetFullDamage();

      ChangeHealth(-totalDamage);

      RenderDamages(attackData);
      // DamageUI.Instance.NewDamage(totalDamage, m_Owner.transform.position);
    }

    private void RenderDamages(AttackData attackData)
    {
      var target = attackData.Target;

      // TODO: add normal positioning
      foreach (var damageType in (DamageType[]) Enum.GetValues(typeof(DamageType)))
      {
        var damage = attackData.GetDamage(damageType);

        if (damage == 0) continue;

        ref var text = ref target.layer.GetBuffer<SegmentDamageText>().Add();

        var randomRange = Random.Range(-0.5f, 0.5f);
        text.source = target.layer.Obj.Create(Pool.Entities, "Prefabs/DamageText",
          new Vector3(randomRange, 0, 0) + target.transform.position + Vector3.up * 2);
        text.source.GetComponentInChildren<TextMeshPro>().text = damage.ToString();
        text.source.GetComponentInChildren<TextMeshPro>().color = DamageTypeColor[(int) damageType];
        text.startTime = Time.time;
      }
    }

    /// <summary>
    ///   Store the stats, which are composed of 4 values : health, strength, agility and defense.
    ///   It also contains elemental protections and boost (1 for each elements defined by the DamageType enum)
    /// </summary>
    [Serializable]
    public class Stats
    {
      //Integer for simplicity, may switch to float later on. For now everything is integer
      public int health;
      public int strength;
      public int defense;
      public int agility;

      //use an array indexed by the DamageType enum for easy extensibility
      public int[] elementalProtection = new int[Enum.GetValues(typeof(DamageType)).Length];
      public int[] elementalBoosts = new int[Enum.GetValues(typeof(DamageType)).Length];

      public void Copy(Stats other)
      {
        health = other.health;
        strength = other.strength;
        defense = other.defense;
        agility = other.agility;

        Array.Copy(other.elementalProtection, elementalProtection, other.elementalProtection.Length);
        Array.Copy(other.elementalBoosts, elementalBoosts, other.elementalBoosts.Length);
      }

      /// <summary>
      ///   Will modify that Stat by the given StatModifier (see StatModifier documentation for how to use them)
      /// </summary>
      /// <param name="modifier"></param>
      public void Modify(StatModifier modifier)
      {
        //bit convoluted, but allow to reuse the normal int stat system for percentage change
        if (modifier.modifierMode == StatModifier.Mode.Percentage)
        {
          health += Mathf.FloorToInt(health * (modifier.stats.health / 100.0f));
          strength += Mathf.FloorToInt(strength * (modifier.stats.strength / 100.0f));
          defense += Mathf.FloorToInt(defense * (modifier.stats.defense / 100.0f));
          agility += Mathf.FloorToInt(agility * (modifier.stats.agility / 100.0f));

          for (var i = 0; i < elementalProtection.Length; ++i)
            elementalProtection[i] +=
              Mathf.FloorToInt(elementalProtection[i] * (modifier.stats.elementalProtection[i] / 100.0f));

          for (var i = 0; i < elementalBoosts.Length; ++i)
            elementalBoosts[i] += Mathf.FloorToInt(elementalBoosts[i] * (modifier.stats.elementalBoosts[i] / 100.0f));
        }
        else
        {
          health += modifier.stats.health;
          strength += modifier.stats.strength;
          defense += modifier.stats.defense;
          agility += modifier.stats.agility;

          for (var i = 0; i < elementalProtection.Length; ++i)
            elementalProtection[i] += modifier.stats.elementalProtection[i];

          for (var i = 0; i < elementalBoosts.Length; ++i)
            elementalBoosts[i] += modifier.stats.elementalBoosts[i];
        }
      }
    }

    /// <summary>
    ///   Can be added to a stack of modifiers on the StatSystem to modify the value of the base stats
    ///   e.g. a weapon adding +2 strength will push a modifier on the top of the stack.
    ///   They have 2 modes : Absolute, where values are added as is, and Percentage, where values are converted to
    ///   percentage (e.g. a value of 50 in strength in a Percentage modifier will increase the strength by 50%).
    /// </summary>
    [Serializable]
    public class StatModifier
    {
      /// <summary>
      ///   The mode of the modifier : Percentage will divide the value by 100 to get a percentage, absolute use the
      ///   value as is.
      /// </summary>
      public enum Mode
      {
        Percentage,
        Absolute
      }

      public Mode modifierMode = Mode.Absolute;
      public Stats stats = new Stats();
    }

    /// <summary>
    ///   This is a special StatModifier, that gets added to the TimedStatModifier stack, that will be automatically
    ///   removed when its timer reaches 0. Contains a StatModifier that controls the actual modification.
    /// </summary>
    [Serializable]
    public class TimedStatModifier
    {
      public string id;
      public StatModifier modifier;

      public Sprite effectSprite;

      public float duration;
      public float timer;

      public void Reset()
      {
        timer = duration;
      }
    }
  }
}