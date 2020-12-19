using ActorsECS.Core.Modules.Character.Components;
using ActorsECS.Core.Modules.Common;
using ActorsECS.Core.UI;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Core.Modules.Shooting.Processors
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
        ref var cWeapon = ref character.ComponentWeapon();

        if (cInput.Reload && !character.Has(Tag.Reload))
        {
          cWeapon.reloadStartTime = UnityEngine.Time.time;

          character.Set(Tag.Reload);

          _reloadUI.StartReload(character.ComponentEquipment().equipmentSystem.Weapon.stats.reloadTime);
        }
      }

      foreach (var character in _reloadingCharacters)
      {
        ref var cWeapon = ref character.ComponentWeapon();
        ref var cEquipment = ref character.ComponentEquipment();

        if (cWeapon.reloadStartTime + cEquipment.equipmentSystem.Weapon.stats.reloadTime <= UnityEngine.Time.time)
        {
          character.ComponentWeapon().currentAmmo = character.ComponentEquipment().equipmentSystem.Weapon.stats.ammo;
          
          character.Remove(Tag.Reload);
        }
      }
    }
  }
}