﻿using ActorsECS.Modules.Enemy.Components;
using Pixeye.Actors;

namespace ActorsECS.Modules.Enemy.Processors
{
  internal sealed class ProcessorEnemyDeath : Processor, ITick
  {
    private readonly Group<ComponentEnemy> _enemies = default;

    public void Tick(float delta)
    {
      foreach (var enemy in _enemies)
      {
        ref var cEnemy = ref enemy.ComponentEnemy();

        if (!(cEnemy.health <= 0)) continue;
        
        enemy.Release();
      }
    }
  }
}