using System;
using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ActorsECS.Modules.Character.Components
{
  [Serializable]
  public struct ComponentRotation
  {
    public Quaternion rotation;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal static partial class Component
  {
    public const string Rotation = "ActorsECS.Modules.Character.Components.ComponentRotation";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ComponentRotation ComponentRotation(in this ent entity)
    {
      return ref Storage<ComponentRotation>.components[entity.id];
    }
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal sealed class StorageComponentRotation : Storage<ComponentRotation>
  {
    public override ComponentRotation Create()
    {
      return new ComponentRotation();
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