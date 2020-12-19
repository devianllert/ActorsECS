using System;
using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;

namespace ActorsECS.Modules.Enemy.Components
{
  [Serializable]
  public struct ComponentEnemy
  {
    public string name;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal static partial class Component
  {
    public const string Enemy = "ActorsECS.Modules.Enemy.Components.ComponentEnemy";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ComponentEnemy ComponentEnemy(in this ent entity)
    {
      return ref Storage<ComponentEnemy>.components[entity.id];
    }
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal sealed class StorageComponentEnemy : Storage<ComponentEnemy>
  {
    public override ComponentEnemy Create()
    {
      return new ComponentEnemy();
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