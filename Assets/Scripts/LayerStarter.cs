using ActorsECS.Core.Modules;
using ActorsECS.Core.Modules.Character.Processors;
using ActorsECS.Core.Modules.Enemy.Processors;
using ActorsECS.Core.Modules.Loot.Processors;
using ActorsECS.Core.Modules.Shooting.Components;
using ActorsECS.Core.Modules.Shooting.Processors;
using Pixeye.Actors;

namespace ActorsECS
{
  public class LayerStarter : Layer<LayerStarter>
  {
    protected override void Setup()
    {
      #region Character

      Add<ProcessorGatheringInput>();
      Add<ProcessorMove>();
      Add<ProcessorRotation>();
      Add<ProcessorAnimation>();
      Add<ProcessorLootCollisions>();
      Add<ProcessorPickup>();
      Add<ProcessorStats>();

      #endregion

      #region Abilities

      Add<ProcessorRoll>();

      #endregion

      #region Shooting

      Add<Buffer<SegmentBullet>>();
      Add<ProcessorShooting>();
      Add<ProcessorProjectiles>();
      Add<ProcessorReload>();

      #endregion

      #region Loot

      Add<ProcessorOutline>();

      #endregion

      #region Enemy

      Add<ProcessorEnemyAI>();
      Add<ProcessorEnemyDeath>();

      #endregion

      #region Common

      Add<Buffer<SegmentDamageText>>();
      Add<ProcessorDamageText>();

      #endregion
    }
  }
}