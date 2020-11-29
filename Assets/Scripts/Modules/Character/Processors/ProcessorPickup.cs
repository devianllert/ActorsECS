using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Loot.Components;
using Pixeye.Actors;

namespace ActorsECS.Modules.Character.Processors
{
  internal sealed class ProcessorPickup : Processor, ITick
  {
    private readonly Group<ComponentInput> _characters = default;

    [GroupBy(Tag.Lootable)] private readonly Group<ComponentLootData> _loots = default;

    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cinput = ref character.ComponentInput();

        if (!cinput.Interact || !_loots[0].exist) continue;

        var loot = _loots[0];

        ref var lootData = ref loot.ComponentLootData();

        lootData.item.Pickup(character, loot);

        loot.Release();
      }
    }
  }
}