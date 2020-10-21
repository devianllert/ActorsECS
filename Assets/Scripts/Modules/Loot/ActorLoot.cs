using ActorsECS.Modules.Loot.Components;
using Pixeye.Actors;

namespace ActorsECS.Modules.Loot
{
  public class ActorLoot : Actor
  {
    [FoldoutGroup("Components", true)]
    public ComponentLootData componentLootData;

    protected override void Setup()
    {
      entity.Set(componentLootData);
    }
  }
}
