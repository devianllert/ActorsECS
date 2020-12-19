using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ActorsECS.Modules.Character.Components
{
  public struct ComponentAim
  {
    public Vector3 point;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal static partial class Component
  {
    public const string Aim = "ActorsECS.Modules.Character.Components.ComponentAim";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ComponentAim ComponentAim(in this ent entity)
    {
      return ref Storage<ComponentAim>.components[entity.id];
    }
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal sealed class StorageComponentAim : Storage<ComponentAim>
  {
    public override ComponentAim Create()
    {
      return new ComponentAim();
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