using System;
using System.Runtime.CompilerServices;
using ActorsECS.Data;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;

namespace ActorsECS.Modules.Common
{
  [Serializable]
  public struct ComponentStats
  {
    public StatSystem StatSystem;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal static partial class Component
  {
    public const string Stats = "Game.Source.ComponentStats";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ComponentStats ComponentStats(in this ent entity)
    {
      return ref Storage<ComponentStats>.components[entity.id];
    }
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal sealed class StorageComponentStats : Storage<ComponentStats>
  {
    public override ComponentStats Create()
    {
      return new ComponentStats();
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