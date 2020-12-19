using ActorsECS.Core.Modules.Character.Components;
using ActorsECS.Core.Modules.Common;
using ActorsECS.Core.UI;
using Pixeye.Actors;
using UnityEngine;
using Random = Pixeye.Actors.Random;

namespace ActorsECS.Core.Modules.Shooting.Processors
{
  internal sealed class ProcessorShooting : Processor, ITick
  {
    private readonly GameObject _ammoUI;
    private readonly CurrentAmmoUI _currentAmmoUI;
    private readonly TotalAmmoUI _totalAmmoUI;
    
    [ExcludeBy(Tag.Reload, Tag.Roll)]
    private readonly Group<ComponentInput, ComponentWeapon> _characters = default;
    
    [ExcludeBy(Tag.Reload)]
    private readonly Group<ComponentWeapon> _entsWithWeapons = default;
    
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

      foreach (var entity in _entsWithWeapons)
      {
        ref var cWeapon = ref entity.ComponentWeapon();
        
        cWeapon.fireTime -= delta;
      }
      
      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();
        ref var cEquipment = ref character.ComponentEquipment().equipmentSystem;
        ref var cWeapon = ref character.ComponentWeapon();

        if (!cEquipment.Weapon) return;

        if (cInput.Shoot > 0)
        {
          cEquipment.Weapon.projectileBehaviour.Launch(character);
        }

        _currentAmmoUI.UpdateCurrentAmmo(cWeapon.currentAmmo);
        _totalAmmoUI.UpdateTotalAmmo(cEquipment.Weapon.stats.ammo);
      }
    }
  }
}