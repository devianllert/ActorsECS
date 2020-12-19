using System;
using System.Runtime.CompilerServices;
using CleverCrow.Fluid.BTs.Trees;
using Pixeye.Actors;
using Unity.IL2CPP.CompilerServices;

namespace ActorsECS.Core.Modules.Enemy.Components
 {
   [Serializable]
   public struct ComponentAI
   {
     public BehaviorTree behavior;
   }
 
   #region HELPERS
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   static partial class Component
   {
     public const string AI = "ActorsECS.Modules.Enemy.Components.ComponentBrain";
     [MethodImpl(MethodImplOptions.AggressiveInlining)]
     public static ref ComponentAI ComponentAI(in this ent entity) =>
       ref Storage<ComponentAI>.components[entity.id];
   }
 
   [Il2CppSetOption(Option.NullChecks, false)]
   [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
   [Il2CppSetOption(Option.DivideByZeroChecks, false)]
   sealed class StorageComponentAI : Storage<ComponentAI>
   {
     public override ComponentAI Create() => new ComponentAI();
     // Use for cleaning components that were removed at the current frame.
     public override void Dispose(indexes disposed)
     {
       foreach (var id in disposed)
       {
         ref var component = ref components[id];

         component.behavior = null;
       }
     }
   }
 
   #endregion
 }
