using System;
using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;

namespace ActorsECS.Modules.Character.Components
 {
   [Serializable]
   public struct ComponentMovement
   {
     public float speed;
   }
 
   #region HELPERS
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   static partial class Component
   {
     public const string Movement = "ActorsECS.Modules.Character.Components.ComponentMovement";
     [MethodImpl(MethodImplOptions.AggressiveInlining)]
     public static ref ComponentMovement ComponentMovement(in this ent entity) =>
       ref Storage<ComponentMovement>.components[entity.id];
   }
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   sealed class StorageComponentMovement : Storage<ComponentMovement>
   {
     public override ComponentMovement Create() => new ComponentMovement();
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
