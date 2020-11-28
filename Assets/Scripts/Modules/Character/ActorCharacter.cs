using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Common;
using Pixeye.Actors;

namespace ActorsECS.Modules.Character
{
  public class ActorCharacter : Actor
  {
    [FoldoutGroup("Components", true)]
    private ComponentInput componentInput;
    private ComponentRotation componentRotation;
    private ComponentMovementDirection componentMovementDirection;
    private ComponentAim componentAim;

    public ComponentMovement componentMovement;
    public ComponentRoll componentRoll;
    public ComponentStats componentStats;

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