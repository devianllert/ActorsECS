using System;
using ActorsECS.Data.Items;
using ActorsECS.Data.Systems;
using ActorsECS.VFX;
using Pixeye.Actors;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ActorsECS.Data.Effects
{
  /// <summary>
  ///   The base class to derive from to write you own custom Elemental effect that can be added to a StatsSystem. There
  ///   is a default implementation called ElementalEffect that can be used to make Physical/Fire/Electrical/Cold damage
  ///   across time.
  ///   A derived class *must* implement the Equals function so we can check if 2 effects are the same (e.g. the default
  ///   implementation ElementalEffect will consider 2 effect equal if they do the same DamageType).
  /// </summary>
  public abstract class BaseElementalEffect : IEquatable<BaseElementalEffect>
  {
    protected float _duration;
    protected ent _target;
    protected float _timer;

    public BaseElementalEffect(float duration)
    {
      _duration = duration;
    }

    public bool Done => _timer <= 0.0f;
    public float CurrentTime => _timer;
    public float Duration => _duration;

    public abstract bool Equals(BaseElementalEffect other);

    public virtual void Applied(ent target)
    {
      _timer = _duration;
      _target = target;
    }

    public virtual void Removed()
    {
    }

    public virtual void Tick(StatSystem statSystem)
    {
      _timer -= Time.deltaTime;
    }
  }

  /// <summary>
  ///   Default implementation of the BaseElementalEffect. The constructor allows the caller to specify what type of
  ///   damage is done, how much is done and the speed (time) between each instance of damage (default 1 = every second).
  /// </summary>
  public class ElementalEffect : BaseElementalEffect
  {
    private readonly float _damageSpeed;
    private readonly StatSystem.DamageType _damageType;
    private readonly int _damage;

    private VFXManager.VFXInstance _fireInstance;
    private float _sinceLastDamage;

    public ElementalEffect(float duration, StatSystem.DamageType damageType, int damage, float speed = 1.0f) :
      base(duration)
    {
      _damage = damage;
      _damageType = damageType;
      _damageSpeed = speed;
    }

    public override void Tick(StatSystem statSystem)
    {
      base.Tick(statSystem);

      _sinceLastDamage += Time.deltaTime;

      if (_sinceLastDamage > _damageSpeed)
      {
        _sinceLastDamage = 0;

        var data = new AttackData(_target);

        data.AddDamage(_damageType, _damage);

        statSystem.Damage(data);
      }

      //we do not parent as if the original object is destroy it would destroy the instance
      _fireInstance.Effect.transform.position = _target.transform.position + Vector3.up;
    }

    public override bool Equals(BaseElementalEffect other)
    {
      if (!(other is ElementalEffect eff))
        return false;

      return eff._damageType == _damageType;
    }

    public override void Applied(ent target)
    {
      base.Applied(target);

      //We use the fire effect as it's the only one existing in the project.
      //You can add new VFX and use an if or switch statement to use the right VFXType
      //depending on this effect m_DamageType
      // m_FireInstance = VFXManager.GetVFX(VFXType.FireEffect);
      _fireInstance.Effect.transform.position = target.transform.position + Vector3.up;
    }

    public override void Removed()
    {
      base.Removed();

      _fireInstance.Effect.gameObject.SetActive(false);
    }
  }
}