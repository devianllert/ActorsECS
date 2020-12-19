using ActorsECS.Core.Modules.Common;
using ActorsECS.Core.Modules.Enemy.Components;
using Pixeye.Actors;

namespace ActorsECS.Core.Modules.Enemy.Processors
{
  internal sealed class ProcessorEnemyDeath : Processor, ITick
  {
    private readonly Group<ComponentEnemy> _enemies = default;

    public void Tick(float delta)
    {
      foreach (var enemy in _enemies)
      {
        ref var cHealth = ref enemy.ComponentStats().statSystem;

        if (!(cHealth.CurrentHealth <= 0)) continue;

        enemy.Release();
      }
    }
  }
}