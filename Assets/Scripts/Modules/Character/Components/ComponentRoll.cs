using System;
using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;

namespace ActorsECS.Modules.Character.Components
 {
   [Serializable]
   public struct ComponentRoll
   {
     public float duration;
     public float distance;
     public float cooldown;
     public float elapsedCooldown;
     public float elapsedDuration;
   }
 
   #region HELPERS
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   static partial class Component
   {
     public const string Roll = "ActorsECS.Modules.Character.Components.ComponentRoll";
     [MethodImpl(MethodImplOptions.AggressiveInlining)]
     public static ref ComponentRoll ComponentRoll(in this ent entity) =>
       ref Storage<ComponentRoll>.components[entity.id];
   }
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   sealed class StorageComponentRoll : Storage<ComponentRoll>
   {
     public override ComponentRoll Create() => new ComponentRoll();
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
