using ActorsECS.Modules.Common;
using ActorsECS.Modules.Enemy.Components;
using Pixeye.Actors;

namespace ActorsECS.Modules.Enemy
{
  public class ActorEnemy : Actor
  {
    [FoldoutGroup("Components", true)]
    public ComponentHealth componentHealth;
    public ComponentEnemy componentEnemy;

    protected override void Setup()
    {
      entity.Set(componentEnemy);
      entity.Set(componentHealth);
    }
  }
}