using System;
using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ActorsECS.Modules.Character.Components
{
  [Serializable]
  public struct ComponentInput
  {
    public Vector2 Movement;
    public bool Interact;
    public float Shoot;
    public bool Reload;
    public bool Roll;
    public bool Pause;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal static partial class Component
  {
    public const string Input = "ActorsECS.Modules.Character.Components.ComponentInput";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ComponentInput ComponentInput(in this ent entity)
    {
      return ref Storage<ComponentInput>.components[entity.id];
    }
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal sealed class StorageComponentInput : Storage<ComponentInput>
  {
    public override ComponentInput Create()
    {
      return new ComponentInput();
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