﻿using System;
using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Modules.Character.Components
 {
   [Serializable]
   public struct ComponentInput
   {
     public Vector2 movement;
   }
 
   #region HELPERS
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   static partial class Component
   {
     public const string Input = "Game.Source.ComponentInput";
     [MethodImpl(MethodImplOptions.AggressiveInlining)]
     public static ref ComponentInput CharacterInputData(in this ent entity) =>
       ref Storage<ComponentInput>.components[entity.id];
   }
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   sealed class StorageCharacterInputData : Storage<ComponentInput>
   {
     public override ComponentInput Create() => new ComponentInput();
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