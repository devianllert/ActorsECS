using ActorsECS.Modules.Character.Processors;
using ActorsECS.Modules.Loot.Processors;
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

      #endregion

      #region Loot

      Add<ProcessorOutline>();

      #endregion
    }
  }
}