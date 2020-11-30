using ActorsECS.Modules.Shooting.Components;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Data.Projectiles
{
  /// <summary>
  ///   Base class of all projectiles in the game. This is an abstract class and need to be inherited to specify behaviour.
  /// </summary>
  public abstract class Projectile : ScriptableObject
  {
    public GameObject worldObjectPrefab;

    public virtual void Tick(ent character, ref SegmentBullet bullet, int pointer)
    {
    }

    public virtual void Destroy(ent character, int pointer)
    {
    }

    public virtual void Launch(ent character)
    {
    }
  }
}