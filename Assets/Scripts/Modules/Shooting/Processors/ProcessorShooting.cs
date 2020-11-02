using ActorsECS.Data;
using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Shooting.Components;
using ActorsECS.UI;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Shooting.Processors
{
  internal sealed class ProcessorShooting : Processor, ITick
  {
    private readonly CurrentAmmoUI _currentAmmoUI;
    private readonly TotalAmmoUI _totalAmmoUI;

    private readonly Group<ComponentInput> _characters = default;

    public ProcessorShooting()
    {
      _currentAmmoUI = Object.FindObjectOfType<CurrentAmmoUI>();
      _totalAmmoUI = Object.FindObjectOfType<TotalAmmoUI>();
    }
    
    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();
        ref var cWeapon = ref character.ComponentWeapon();
        ref var cRotation = ref character.ComponentRotation();
        var transform = character.GetMono<Transform>();

        var bulletType = (int) cWeapon.equippedWeapon.bulletType;

        cWeapon.fireTime -= delta;

        if (cInput.Shoot > 0 && cWeapon.fireTime <= 0 && cWeapon.currentAmmo != 0 && !cWeapon.isReloading)
        {
          cWeapon.fireTime = 60 / cWeapon.equippedWeapon.rateOfFire;

          switch (bulletType)
          {
            case 0:
              CreateBullet(cWeapon.equippedWeapon, transform, cRotation.rotation);
              break;
            case 1:
              CreateLaser(cWeapon.equippedWeapon, transform, cRotation.rotation);
              break;
          }

          cWeapon.currentAmmo -= 1;
        }
        
        _currentAmmoUI.UpdateCurrentAmmo(cWeapon.currentAmmo);
        _totalAmmoUI.UpdateTotalAmmo(cWeapon.equippedWeapon.ammo);
      }
    }

    private void CreateBullet(Weapon weapon, Transform transform, Quaternion rotation)
    {
      ref var bullet = ref Layer.GetBuffer<SegmentBullet>().Add();
      
      bullet.position = transform.position + Vector3.up + transform.forward;
      bullet.speed = weapon.speed;
      bullet.source = Obj.Create(Pool.Entities, $"Prefabs/Projectiles/{weapon.bulletPrefabName}", bullet.position);
      bullet.distance = 0f;
      bullet.direction = rotation;
      bullet.range = weapon.range;
      bullet.damage = weapon.damage;
    }

    private void CreateLaser(Weapon weapon, Transform transform, Quaternion rotation)
    {
      ref var laser = ref Layer.GetBuffer<SegmentLaser>().Add();
      
      laser.position = transform.position + Vector3.up + transform.forward;
      laser.source = Obj.Create(Pool.Entities, "Prefabs/Projectiles/Laser", laser.position);
      laser.direction = rotation;
      laser.range = weapon.range;
    }
  }
}