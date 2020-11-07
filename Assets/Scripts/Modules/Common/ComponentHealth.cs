using System;
using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;

namespace ActorsECS.Modules.Common
 {
   [Serializable]
   public struct ComponentHealth
   {
     public float health;
   }
 
   #region HELPERS
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   static partial class Component
   {
     public const string Health = "ActorsECS.Modules.Common.ComponentHealth";
     [MethodImpl(MethodImplOptions.AggressiveInlining)]
     public static ref ComponentHealth ComponentHealth(in this ent entity) =>
       ref Storage<ComponentHealth>.components[entity.id];
   }

   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   sealed class StorageComponentHealth : Storage<ComponentHealth>
   {
     public override ComponentHealth Create() => new ComponentHealth();
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
 
