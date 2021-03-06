﻿using ActorsECS.Modules.Loot.Components;
using ActorsECS.UI;
using Pixeye.Actors;

namespace ActorsECS.Modules.Loot.Processors
{
  internal sealed class ProcessorOutline : Processor, ITick
  {
    [ExcludeBy(Tag.Lootable)] private readonly Group<ComponentLootData> _allLoots = default;

    [GroupBy(Tag.Lootable)] private readonly Group<ComponentLootData> _loots = default;

    public void Tick(float dt)
    {
      foreach (var loot in _loots)
      {
        var outline = loot.GetMono<Outline>(0);

        outline.enabled = true;

        InteractUI.Instance.ShowTooltip(outline.transform.position);
      }

      foreach (var loot in _allLoots)
      {
        var outline = loot.GetMono<Outline>(0);

        outline.enabled = false;
      }

      if (_loots.length == 0 && InteractUI.Instance.IsEnabled) InteractUI.Instance.HideTooltip();
    }
  }
}