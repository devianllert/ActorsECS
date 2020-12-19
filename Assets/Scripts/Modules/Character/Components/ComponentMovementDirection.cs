using System;
using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ActorsECS.Modules.Character.Components
{
  [Serializable]
  public struct ComponentMovementDirection
  {
    public Vector2 direction;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal static partial class Component
  {
    public const string MovementDirection = "ActorsECS.Modules.Character.Components.ComponentMovementDirection";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ComponentMovementDirection ComponentMovementDirection(in this ent entity)
    {
      return ref Storage<ComponentMovementDirection>.components[entity.id];
    }
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal sealed class StorageComponentMovementDirection : Storage<ComponentMovementDirection>
  {
    public override ComponentMovementDirection Create()
    {
      return new ComponentMovementDirection();
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