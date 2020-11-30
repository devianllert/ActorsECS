using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Common;
using ActorsECS.Modules.Shooting.Components;
using ActorsECS.VFX;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Shooting.Processors
{
  internal sealed class ProcessorProjectiles : Processor, ITick
  {
    private readonly Group<ComponentInput, ComponentWeapon> _characters = default;
    private Buffer<SegmentBullet> _bullets => Layer.GetBuffer<SegmentBullet>();

    public void Tick(float delta)
    {
      foreach (var pointer in _bullets)
      {
        ref var cEquipment = ref _characters[0].ComponentEquipment();
        
        cEquipment.equipmentSystem.Weapon.projectile.Tick(_characters[0], ref _bullets[pointer], pointer);
      }
    }
  }
}