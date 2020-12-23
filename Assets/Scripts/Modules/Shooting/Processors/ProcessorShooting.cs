using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Common;
using ActorsECS.UI;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Shooting.Processors
{
  internal sealed class ProcessorShooting : Processor, ITick
  {
    private readonly GameObject _ammoUI;
    
    [ExcludeBy(Tag.Reload, Tag.Roll)]
    private readonly Group<ComponentInput, ComponentWeapon> _characters = default;
    
    [ExcludeBy(Tag.Reload)]
    private readonly Group<ComponentWeapon> _entsWithWeapons = default;
    
    private readonly Group<ComponentInput, ComponentWeapon> _weapon = default;

    public ProcessorShooting()
    {
      _ammoUI = CurrentAmmoUI.Instance.transform.parent.gameObject;
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

        CurrentAmmoUI.Instance.UpdateCurrentAmmo(cWeapon.currentAmmo);
        TotalAmmoUI.Instance.UpdateTotalAmmo(cEquipment.Weapon.stats.ammo);
      }
    }
  }
}