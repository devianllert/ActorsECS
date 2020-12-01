using ActorsECS.Data;
using ActorsECS.Data.Items;
using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Common;
using ActorsECS.Modules.Shooting.Components;
using ActorsECS.UI;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Shooting.Processors
{
  internal sealed class ProcessorShooting : Processor, ITick
  {
    private readonly GameObject _ammoUI;
    private readonly CurrentAmmoUI _currentAmmoUI;
    private readonly TotalAmmoUI _totalAmmoUI;
    
    [ExcludeBy(Tag.Reload, Tag.Roll)]
    private readonly Group<ComponentInput, ComponentWeapon> _characters = default;
    
    private readonly Group<ComponentInput, ComponentWeapon> _weapon = default;

    public ProcessorShooting()
    {
      _currentAmmoUI = Object.FindObjectOfType<CurrentAmmoUI>();
      _totalAmmoUI = Object.FindObjectOfType<TotalAmmoUI>();

      _ammoUI = _currentAmmoUI.transform.parent.gameObject;
    }

    public void Tick(float delta)
    {
      _ammoUI.SetActive(_weapon.length > 0);

      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();
        ref var cEquipment = ref character.ComponentEquipment().equipmentSystem;
        ref var cWeapon = ref character.ComponentWeapon();

        if (!cEquipment.Weapon) return;

        cWeapon.fireTime -= delta;

        if (cInput.Shoot > 0 && cWeapon.fireTime <= 0 && cWeapon.currentAmmo != 0)
        {
          cWeapon.fireTime = 60 / cEquipment.Weapon.stats.rateOfFire;

          cEquipment.Weapon.projectileBehaviour.Launch(character);

          cWeapon.currentAmmo -= 1;
        }

        _currentAmmoUI.UpdateCurrentAmmo(cWeapon.currentAmmo);
        _totalAmmoUI.UpdateTotalAmmo(cEquipment.Weapon.stats.ammo);
      }
    }
  }
}