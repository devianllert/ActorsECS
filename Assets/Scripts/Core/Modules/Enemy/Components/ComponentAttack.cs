using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;

namespace ActorsECS.Core.Modules.Enemy.Components
{
  public struct ComponentDefault
  {
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal static partial class Component
  {
    public const string Default = "Game.Source.ComponentDefault";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ComponentDefault ComponentDefault(in this ent entity)
    {
      return ref Storage<ComponentDefault>.components[entity.id];
    }
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal sealed class StorageComponentDefault : Storage<ComponentDefault>
  {
    public override ComponentDefault Create()
    {
      return new ComponentDefault();
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