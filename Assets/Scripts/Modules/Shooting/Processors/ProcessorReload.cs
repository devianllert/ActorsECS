using System.Collections;
using ActorsECS.Modules.Character.Components;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Shooting.Processors
{
  internal sealed class ProcessorReload : Processor, ITick
  {
    private readonly ReloadUI _reloadUI;
    
    private readonly Group<ComponentInput> _characters = default;

    public ProcessorReload()
    {
      _reloadUI = Object.FindObjectOfType<ReloadUI>();
    }
    
    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cWeapon = ref character.ComponentWeapon();
        ref var cInput = ref character.ComponentInput();

        if (!cWeapon.isReloading && cInput.Reload)
        {
          cWeapon.isReloading = true;

          Layer.Run(StartReload(character, cWeapon.equippedWeapon.reloadTime));
        }
      }
    }

    private IEnumerator StartReload(ent character, float time)
    {
      _reloadUI.StartReload(time);
      
      yield return Layer.Wait(time);

      character.ComponentWeapon().isReloading = false;

      character.ComponentWeapon().currentAmmo = character.ComponentWeapon().equippedWeapon.ammo;
    }
  }
}