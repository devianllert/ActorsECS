using ActorsECS.Core;
using ActorsECS.Core.Modules.Character.Components;
using ActorsECS.Core.Modules.Common;
using ActorsECS.Core.Modules.Shooting.Components;
using ActorsECS.Core.VFX;
using Pixeye.Actors;
using UnityEngine;
using Random = Pixeye.Actors.Random;
using Time = UnityEngine.Time;

namespace ActorsECS.Data.Projectiles
{
  [CreateAssetMenu(fileName = "SlowBulletsProjectileBehaviour", menuName = "Data/Create/Projectiles/BulletsProjectileBehaviour", order = 0)]
  public class BulletsProjectileBehaviour : ProjectileBehaviour
  {
    public override void Launch(ent character)
    {
      ref var cEquipment = ref character.ComponentEquipment().equipmentSystem;
      ref var cWeapon = ref character.ComponentWeapon();
      ref var projectilePoint = ref character.ComponentWeapon().projectilePoint;

      if (cWeapon.fireTime <= 0 && cWeapon.currentAmmo != 0)
      {
        cWeapon.fireTime = 60 / cEquipment.Weapon.stats.rateOfFire;

        SFXManager.PlaySound(SFXManager.Use.Player, new SFXManager.PlayData()
        {
          Clip = cEquipment.Weapon.shootSounds[Random.Range(0, cEquipment.Weapon.shootSounds.Length)], 
          Position = character.transform.position,
        });
        
        cWeapon.currentAmmo -= 1;

        ref var bullet = ref character.layer.GetBuffer<SegmentBullet>().Add();

        bullet.owner = character;
        bullet.weapon = cEquipment.Weapon;
        bullet.position = projectilePoint.position;
        bullet.speed = cEquipment.Weapon.stats.speed;
        bullet.source = Layer<LayerStarter>.Obj.Create(Pool.Entities, worldObjectPrefab, bullet.position);
        bullet.distance = 0f;
        bullet.direction = character.transform.rotation;
        bullet.range = cEquipment.Weapon.stats.range;
      }
    }

    public override void Destroy(int pointer)
    {
      var bullets = Layer<LayerStarter>.GetBuffer<SegmentBullet>();
      
      ref var bullet = ref bullets[pointer];
      
      var vfx = VFXManager.PlayVFX(VFXType.BulletHit, bullet.source.position);

      bullet.source.gameObject.Release(Pool.Entities);
      bullets.RemoveAt(pointer);

      Layer<LayerStarter>.WaitFor(0.5f, () => vfx.Effect.gameObject.SetActive(false));
    }

    public override void Tick(ref SegmentBullet bullet, int pointer)
    {
      bullet.distance += bullet.speed * Time.deltaTime;

      bullet.source.rotation = bullet.direction;

      var positionIncrement = bullet.source.forward * bullet.speed * Time.deltaTime;

      if (Physics.Raycast(bullet.source.position, positionIncrement.normalized, out var hit,
        positionIncrement.magnitude, LayerMask.GetMask("Player", "Enemy", "Environment")))
      {
        var actor = hit.transform.gameObject.GetComponent<Actor>();

        if (actor)
        {
          bullet.weapon.Attack(bullet.owner, actor.entity);

          bullet.weapon.projectileBehaviour.Destroy(pointer);

          return;
        }

        bullet.weapon.projectileBehaviour.Destroy(pointer);

        return;
      }

      bullet.source.position += positionIncrement;

      if (bullet.distance >= bullet.range) bullet.weapon.projectileBehaviour.Destroy(pointer);
    }
  }
}