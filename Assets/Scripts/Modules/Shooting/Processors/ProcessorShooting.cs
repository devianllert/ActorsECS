using System;
using ActorsECS.Data;
using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Shooting.Components;
using Pixeye.Actors;
using UnityEngine;
using Random = Pixeye.Actors.Random;

namespace ActorsECS.Modules.Shooting.Processors
{
  internal sealed class ProcessorShooting : Processor, ITick
  {
    private readonly Group<ComponentInput> _characters = default;
    
    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cinput = ref character.ComponentInput();
        ref var cweapon = ref character.ComponentWeapon();
        var transform = character.GetMono<Transform>();

        var bulletType = (int) cweapon.weapon.bullet;
        
        cweapon.fireTime -= delta;
        
        if (cinput.fired > 0 && !cweapon.reload && cweapon.fireTime <= 0)
        {
          cweapon.fireTime = 60 / cweapon.weapon.rate;

          switch (bulletType)
          {
            case 0:
              CreateBullet(cweapon.weapon, transform, character.ComponentRotation().rotation);
              break;
            case 1:
              CreateLaser(cweapon.weapon, transform, character.ComponentRotation().rotation);
              break;
          }
        }
      }
    }

    private void CreateBullet(Weapon weapon, Transform transform, Quaternion rotation)
    {
      ref var bullet = ref Layer.GetBuffer<SegmentBullet>().Add();
      bullet.position = transform.position + Vector3.up + transform.forward;
      bullet.speed = weapon.speed;
      bullet.source = Obj.Create(Pool.Entities, "Prefabs/Bullet", bullet.position);
      bullet.distance = 0f;
      bullet.direction = rotation;
      bullet.range = weapon.range;
      bullet.damage = weapon.damage;
    }
    
    private void CreateLaser(Weapon weapon, Transform transform, Quaternion rotation)
    {
      ref var laser = ref Layer.GetBuffer<SegmentLaser>().Add();
      laser.position = transform.position + Vector3.up + transform.forward;
      laser.source = Obj.Create(Pool.Entities, "Prefabs/Laser", laser.position);
      laser.direction = rotation;
      laser.range = weapon.range;
    }
  }
}
