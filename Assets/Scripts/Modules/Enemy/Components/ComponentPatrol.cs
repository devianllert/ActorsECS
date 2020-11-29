using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ActorsECS.Modules.Enemy.Components
{
  public struct ComponentPatrol
  {
    public Vector3 points;
    public float speed;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal static partial class Component
  {
    public const string Patrol = "ActorsECS.Modules.Enemy.Components.ComponentPatrol";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ComponentPatrol ComponentPatrol(in this ent entity)
    {
      return ref Storage<ComponentPatrol>.components[entity.id];
    }
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal sealed class StorageComponentPatrol : Storage<ComponentPatrol>
  {
    public override ComponentPatrol Create()
    {
      return new ComponentPatrol();
    }

    // Use for cleaning components that were removed at the current frame.
    public override void Dispose(indexes disposed)
    {
      foreach (var id in disposed)
      {
        ref var component = ref components[id];
      }
    }
  }

  #endregion
}