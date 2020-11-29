using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Common;
using Pixeye.Actors;

namespace ActorsECS.Modules.Character
{
  public class ActorCharacter : Actor
  {
    public ComponentMovement componentMovement;
    public ComponentRoll componentRoll;
    public ComponentStats componentStats;
    private ComponentAim componentAim;
    [FoldoutGroup("Components", true)] private ComponentInput componentInput;
    private ComponentMovementDirection componentMovementDirection;
    private ComponentRotation componentRotation;

    protected override void Setup()
    {
      componentStats.StatSystem.stats.health = 100;
      componentStats.StatSystem.Init(entity);

      entity.Set(componentInput);
      entity.Set(componentMovement);
      entity.Set(componentRotation);
      entity.Set(componentMovementDirection);
      entity.Set(componentRoll);
      entity.Set(componentStats);
      entity.Set(componentAim);
    }
  }
}