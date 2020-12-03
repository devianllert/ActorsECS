using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Common;
using ActorsECS.Modules.Shooting.Components;
using ActorsECS.VFX;
using Pixeye.Actors;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ActorsECS.Data.Projectiles
{
  [CreateAssetMenu(fileName = "SlowBulletsProjectileBehaviour", menuName = "Data/Create/Projectiles/SlowBulletsProjectileBehaviour", order = 0)]
  public class SlowBulletsProjectileBehaviour : ProjectileBehaviour
  {
    public override void Launch(ent character)
    {
      ref var bullet = ref character.layer.GetBuffer<SegmentBullet>().Add();
      ref var cEquipment = ref character.ComponentEquipment().equipmentSystem;
      ref var cRotation = ref character.ComponentRotation();
      ref var projectilePoint = ref character.ComponentWeapon().projectilePoint;

      bullet.owner = character;
      bullet.weapon = cEquipment.Weapon;
      bullet.position = projectilePoint.position;
      bullet.speed = cEquipment.Weapon.stats.speed;
      bullet.source = character.layer.Obj.Create(Pool.Entities, worldObjectPrefab, bullet.position);
      bullet.distance = 0f;
      bullet.direction = cRotation.rotation;
      bullet.range = cEquipment.Weapon.stats.range;
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
      bullet.distance += (bullet.speed / 5f) * Time.deltaTime;

      bullet.source.rotation = bullet.direction;

      var positionIncrement = bullet.source.forward * (bullet.speed / 5f) * Time.deltaTime;

      if (Physics.Raycast(bullet.source.position, positionIncrement.normalized, out var hit,
        positionIncrement.magnitude, LayerMask.GetMask("Enemy", "Environment")))
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