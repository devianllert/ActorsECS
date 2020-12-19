using System;
using System.Runtime.CompilerServices;
using ActorsECS.Data.Systems;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;

namespace ActorsECS.Core.Modules.Common
 {
   [Serializable]
   public struct ComponentEquipment
   {
     public EquipmentSystem equipmentSystem;
   }
 
   #region HELPERS
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   static partial class Component
   {
     public const string Equipment = "ActorsECS.Modules.Common.ComponentEquipment";
     [MethodImpl(MethodImplOptions.AggressiveInlining)]
     public static ref ComponentEquipment ComponentEquipment(in this ent entity) =>
       ref Storage<ComponentEquipment>.components[entity.id];
   }
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   sealed class StorageComponentEquipment : Storage<ComponentEquipment>
   {
     public override ComponentEquipment Create() => new ComponentEquipment();
     // Use for cleaning components that were removed at the current frame.
     public override void Dispose(indexes disposed)
     {
       foreach (var id in disposed)
       {
         ref var component = ref components[id];

         component.equipmentSystem = null;
       }
     }
   }
 
   #endregion
 }
