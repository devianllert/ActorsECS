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

      if (entity.transform.childCount > 0)
      {
        var outline = entity.transform.GetChild(0).gameObject.AddComponent<Outline>();
        outline.enabled = false;
      }
    }
  }
}