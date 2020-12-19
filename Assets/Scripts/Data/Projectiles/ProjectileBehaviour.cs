using ActorsECS.Core.Modules.Shooting.Components;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Data.Projectiles
{
  /// <summary>
  ///   Base class of all projectiles in the game. This is an abstract class and need to be inherited to specify behaviour.
  /// </summary>
  public abstract class ProjectileBehaviour : ScriptableObject
  {
    public GameObject worldObjectPrefab;

    public virtual void Tick(ref SegmentBullet bullet, int pointer)
    {
    }

    public virtual void Destroy(int pointer)
    {
    }

    public virtual void Launch(ent character)
    {
    }
    
    public virtual void CheckCollision(ref SegmentBullet bullet) {}
  }
}