using ActorsECS.Modules.Enemy.Components;
using Pixeye.Actors;

namespace ActorsECS.Modules.Enemy
{
  public class ActorEnemy : Actor
  {
    [FoldoutGroup("Components", true)]
    public ComponentEnemy componentEnemy;
    
    protected override void Setup()
    {
      componentEnemy.health = 20;
      
      entity.Set(componentEnemy);
    }
  }
}