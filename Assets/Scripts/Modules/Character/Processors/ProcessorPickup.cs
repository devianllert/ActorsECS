using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Loot.Components;
using Pixeye.Actors;
using UnityEngine;

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

        if (!cinput.Interact) continue;
        
        foreach (var loot in _loots)
        {
          var lootData = loot.ComponentLootData();
          
          Debug.Log($"Picked up {lootData.name}");

          loot.Release();

          break;
        }
      }
    }
  }
}