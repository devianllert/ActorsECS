using System.Collections;
using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Common;
using ActorsECS.UI;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Shooting.Processors
{
  internal sealed class ProcessorReload : Processor, ITick
  {
    private readonly Group<ComponentInput, ComponentWeapon> _characters = default;

    [GroupBy(Tag.Reload)] private readonly Group<ComponentWeapon> _reloadingCharacters = default;
    private readonly ReloadUI _reloadUI;

    public ProcessorReload()
    {
      _reloadUI = Object.FindObjectOfType<ReloadUI>();
    }

    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();

        if (cInput.Reload && !character.Has(Tag.Reload)) character.Set(Tag.Reload);
      }
    }

    public override void HandleEcsEvents()
    {
      foreach (var character in _reloadingCharacters.added) Layer.Run(StartReload(character));
    }

    private IEnumerator StartReload(ent character)
    {
      _reloadUI.StartReload(character.ComponentEquipment().equipmentSystem.Weapon.stats.reloadTime);

      yield return Layer.Wait(character.ComponentEquipment().equipmentSystem.Weapon.stats.reloadTime);

      character.ComponentWeapon().currentAmmo = character.ComponentEquipment().equipmentSystem.Weapon.stats.ammo;

      character.Remove(Tag.Reload);
    }
  }
}