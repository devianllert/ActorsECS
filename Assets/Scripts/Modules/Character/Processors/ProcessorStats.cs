using ActorsECS.Modules.Common;
using Pixeye.Actors;

namespace ActorsECS.Modules.Character.Processors
{
  internal sealed class ProcessorStats : Processor, ITick
  {
    private readonly Group<ComponentStats> _characters = default;

    public void Tick(float deltaTime)
    {
      foreach (var character in _characters)
      {
        ref var cStats = ref character.ComponentStats();

        cStats.statSystem.Tick(deltaTime);
      }
    }
  }
}