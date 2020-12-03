using ActorsECS.Data;
using ActorsECS.Modules.Common;
using ActorsECS.Modules.Enemy.Components;
using Pixeye.Actors;

namespace ActorsECS.Modules.Enemy
{
  public class ActorEnemy : Actor
  {
    [FoldoutGroup("Components", true)] public ComponentEnemy componentEnemy;
    public ComponentStats componentStats;

    protected override void Setup()
    {
      componentEnemy.state = EnemyState.Idle;
      componentStats.statSystem.Init(entity);
      
      entity.Set(componentEnemy);
      entity.Set(componentStats);
    }
  }
}