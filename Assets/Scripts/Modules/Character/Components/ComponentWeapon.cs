﻿using System;
using System.Runtime.CompilerServices;
using ActorsECS.Data;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;

namespace ActorsECS.Modules.Character.Components
{
  [Serializable]
  public struct ComponentWeapon
  {
    public WeaponItem equippedWeapon;
    public float fireTime;
    public int currentAmmo;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal static partial class Component
  {
    public const string Weapon = "ActorsECS.Modules.Character.Components.ComponentWeapon";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ComponentWeapon ComponentWeapon(in this ent entity)
    {
      return ref Storage<ComponentWeapon>.components[entity.id];
    }
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  internal sealed class StorageComponentWeapon : Storage<ComponentWeapon>
  {
    public override ComponentWeapon Create()
    {
      return new ComponentWeapon();
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