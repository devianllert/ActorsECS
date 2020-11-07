using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Enemy.Components;
using Pixeye.Actors;
using Unity.Mathematics;
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
        foreach (var character in _characters)
        {
          var characterTransform = character.GetMono<Transform>();
          var enemyTransform = enemy.GetMono<Transform>();

          var position = characterTransform.position;
          enemyTransform.LookAt(math.distance(enemyTransform.position, position) <= 10f
            ? position
            : Vector3.forward);
        }
      }
    }
  }
}