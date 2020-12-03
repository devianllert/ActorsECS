using System;
using ActorsECS.Data;
using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Enemy.Components;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Enemy.Processors
{
  internal sealed class ProcessorEnemyAI : Processor, ITick
  {
    private readonly Group<ComponentInput> _characters = default;

    private readonly Group<ComponentEnemy> _enemies = default;

    public void Tick(float delta)
    {
      foreach (var enemy in _enemies)
      {
        ref var enemyState = ref enemy.ComponentEnemy().state;
        //
        // switch (enemyState)
        // {
        //   case EnemyState.Idle:
        //     Debug.Log("Enemy in idle state");
        //     break;
        //   case EnemyState.Attack:
        //     Debug.Log("Enemy in attack state");
        //     break;
        //   case EnemyState.Patrol:
        //     Debug.Log("Enemy in patrol state");
        //     break;
        //   case EnemyState.Chase:
        //     Debug.Log("Enemy in chase state");
        //     break;
        //   default:
        //     throw new ArgumentOutOfRangeException();
        // }
      }
    }
  }
}