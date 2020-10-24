using ActorsECS.Modules.Enemy.Components;
using Pixeye.Actors;

namespace ActorsECS.Modules.Enemy.Processors
{
  sealed class ProcessorEnemyAI : Processor, ITick
  {
    private readonly Group<ComponentEnemy> _enemies = default;
    
    public void Tick(float delta)
    {
      foreach (var enemy in _enemies)
      {
        
      }
    }
  }
}
