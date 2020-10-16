using Modules.Character.Components;
using Pixeye.Actors;

namespace Modules.Character
{
  public class ActorCharacter : Actor
  {
    [FoldoutGroup("Components", true)]
    public ComponentInput componentInput;
    public ComponentRotation componentRotation;
    public ComponentMovement componentMovement;
    public ComponentMovementDirection componentMovementDirection;
    
    protected override void Setup()
    {
      entity.Set(componentInput);
      entity.Set(componentMovement);
      entity.Set(componentRotation);
      entity.Set(componentMovementDirection);
    }
  }
}
