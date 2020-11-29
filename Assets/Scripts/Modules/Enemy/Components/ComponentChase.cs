using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;

namespace ActorsECS.Modules.Enemy.Components
{
  public struct ComponentChase
  {
    public float speed;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal static partial class Component
  {
    public const string Chase = "Game.Source.ComponentChase";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ComponentChase ComponentChase(in this ent entity)
    {
      return ref Storage<ComponentChase>.components[entity.id];
    }
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal sealed class StorageComponentChase : Storage<ComponentChase>
  {
    public override ComponentChase Create()
    {
      return new ComponentChase();
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