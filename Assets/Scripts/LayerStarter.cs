using Modules.Character.Processors;
using Pixeye.Actors;

public class LayerStarter : Layer<LayerStarter>
{
  protected override void Setup()
  {
    Add<ProcessorGatheringInput>();
    Add<ProcessorMove>();
    Add<ProcessorRotation>();
    Add<ProcessorAnimation>();
  }
}