﻿using System.Collections;
using ActorsECS.Modules.Character.Components;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Shooting.Processors
{
  internal sealed class ProcessorReload : Processor, ITick
  {
    private readonly ReloadUI _reloadUI;
    
    private readonly Group<ComponentInput> _characters = default;
    
    [GroupBy(Tag.Reload)]
    private readonly Group<ComponentWeapon> _reloadingCharacters = default;

    public ProcessorReload()
    {
      _reloadUI = Object.FindObjectOfType<ReloadUI>();
    }
    
    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();

        if (cInput.Reload) character.Set(Tag.Reload);
      }
    }

    public override void HandleEcsEvents()
    {
      foreach (var character in _reloadingCharacters.added)
      {
        Layer.Run(StartReload(character));
      }
    }

    private IEnumerator StartReload(ent character)
    {
      _reloadUI.StartReload(character.ComponentWeapon().equippedWeapon.reloadTime);
      
      yield return Layer.Wait(character.ComponentWeapon().equippedWeapon.reloadTime);

      character.ComponentWeapon().isReloading = false;

      character.ComponentWeapon().currentAmmo = character.ComponentWeapon().equippedWeapon.ammo;
      
      character.Remove(Tag.Reload);
    }
  }
}