using System;
using System.Runtime.CompilerServices;
using ActorsECS.Data;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;

namespace ActorsECS.Modules.Loot.Components
{
  [Serializable]
  public struct ComponentLootData
  {
    public Item item;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal static partial class Component
  {
    public const string LootData = "ActorsECS.Modules.Loot.Components.ComponentLootData";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ComponentLootData ComponentLootData(in this ent entity)
    {
      return ref Storage<ComponentLootData>.components[entity.id];
    }
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal sealed class StorageComponentName : Storage<ComponentLootData>
  {
    public override ComponentLootData Create()
    {
      return new ComponentLootData();
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