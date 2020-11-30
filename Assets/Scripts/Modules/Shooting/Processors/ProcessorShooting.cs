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
        ref var cRotation = ref character.ComponentRotation();
        var transform = character.GetMono<Transform>();

        if (!cEquipment.Weapon) return;

        var bulletType = cEquipment.Weapon.bulletType;

        cWeapon.fireTime -= delta;

        if (cInput.Shoot > 0 && cWeapon.fireTime <= 0 && cWeapon.currentAmmo != 0)
        {
          cWeapon.fireTime = 60 / cEquipment.Weapon.stats.rateOfFire;

          switch (bulletType)
          {
            case 0:
              CreateBullet(cEquipment.Weapon, transform, cRotation.rotation);
              break;
            case 1:
              CreateLaser(cEquipment.Weapon, transform, cRotation.rotation);
              break;
          }

          cWeapon.currentAmmo -= 1;
        }

        _currentAmmoUI.UpdateCurrentAmmo(cWeapon.currentAmmo);
        _totalAmmoUI.UpdateTotalAmmo(cEquipment.Weapon.stats.ammo);
      }
    }

    private void CreateBullet(WeaponItem weapon, Transform transform, Quaternion rotation)
    {
      ref var bullet = ref Layer.GetBuffer<SegmentBullet>().Add();

      bullet.position = transform.position + Vector3.up + transform.forward;
      bullet.speed = weapon.stats.speed;
      bullet.source = Obj.Create(Pool.Entities, weapon.projectilePrefab, bullet.position);
      bullet.distance = 0f;
      bullet.direction = rotation;
      bullet.range = weapon.stats.range;
    }

    private void CreateLaser(WeaponItem weapon, Transform transform, Quaternion rotation)
    {
      ref var laser = ref Layer.GetBuffer<SegmentLaser>().Add();

      laser.position = transform.position + Vector3.up + transform.forward;
      laser.source = Obj.Create(Pool.Entities, weapon.projectilePrefab, laser.position);
      laser.direction = rotation;
      laser.range = weapon.stats.range;
    }
  }
}