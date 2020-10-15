using Game.Source;
using Modules.Character.Components;
using Pixeye.Actors;

namespace Modules.Character
{
  public class ActorCharacter : Actor
  {
    [FoldoutGroup("Components", true)]
    public ComponentInput componentInput;
    public ComponentRotation componentRotation;
    
    protected override void Setup()
    {
      entity.Set(componentInput);
      entity.Set(componentRotation);
    }
  }
}
