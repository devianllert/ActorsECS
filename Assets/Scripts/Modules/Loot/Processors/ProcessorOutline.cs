using ActorsECS.Modules.Loot.Components;
using ActorsECS.UI;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Loot.Processors
{
  internal sealed class ProcessorOutline : Processor, ITick
  {
    private LootUI lootUi;

    [GroupBy(Tag.Lootable)] private readonly Group<ComponentLootData> _loots = default;

    public ProcessorOutline()
    {
      lootUi = Object.FindObjectOfType<LootUI>();
    }

    public override void HandleEcsEvents()
    {
      foreach (var loot in _loots.added) Debug.Log("Added");

      foreach (var loot in _loots.removed)
      {
        Debug.Log("Removed");

        var outline = loot.GetMono<Outline>();

        outline.enabled = false;
      }
    }

    public void Tick(float dt)
    {
      foreach (var loot in _loots)
      {
        var outline = loot.GetMono<Outline>();

        outline.enabled = true;

        lootUi.ShowTooltip(outline.gameObject.transform.position);
      }

      if (_loots.length == 0 && lootUi.IsEnabled) lootUi.HideTooltip();
    }
  }
}