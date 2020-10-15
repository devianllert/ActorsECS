﻿using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using Pixeye.Actors;
using UnityEngine;


namespace Game.Source
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
   static partial class Component
   {
     public const string Rotation = "Game.Source.ComponentRotation";
     [MethodImpl(MethodImplOptions.AggressiveInlining)]
     public static ref ComponentRotation ComponentRotation(in this ent entity) =>
       ref Storage<ComponentRotation>.components[entity.id];
   }
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   sealed class StorageComponentRotation : Storage<ComponentRotation>
   {
     public override ComponentRotation Create() => new ComponentRotation();
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