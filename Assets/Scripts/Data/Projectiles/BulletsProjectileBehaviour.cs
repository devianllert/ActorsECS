using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Common;
using ActorsECS.Modules.Shooting.Components;
using ActorsECS.VFX;
using Pixeye.Actors;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ActorsECS.Data.Projectiles
{
  [CreateAssetMenu(fileName = "BulletsProjectileBehaviour", menuName = "Data/Create/Projectiles/BulletsProjectileBehaviour", order = 0)]
  public class BulletsProjectileBehaviour : ProjectileBehaviour
  {
    public override void Launch(ent character)
    {
      ref var bullet = ref character.layer.GetBuffer<SegmentBullet>().Add();
      ref var cEquipment = ref character.ComponentEquipment().equipmentSystem;
      ref var cRotation = ref character.ComponentRotation();

      var transform = character.GetMono<Transform>();

      bullet.owner = character;
      bullet.weapon = cEquipment.Weapon;
      bullet.position = transform.position + Vector3.up + transform.forward;
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
      ref var cEquipment = ref bullet.owner.ComponentEquipment();

      bullet.distance += bullet.speed * Time.deltaTime;

      bullet.source.rotation = bullet.direction;

      var positionIncrement = bullet.source.forward * bullet.speed * Time.deltaTime;

      if (Physics.Raycast(bullet.source.position, positionIncrement.normalized, out var hit,
        positionIncrement.magnitude, LayerMask.GetMask("Enemy", "Environment")))
      {
        var actor = hit.transform.gameObject.GetComponent<Actor>();

        if (actor)
        {
          cEquipment.equipmentSystem.Weapon.Attack(bullet.owner, actor.entity);

          cEquipment.equipmentSystem.Weapon.projectileBehaviour.Destroy(pointer);

          return;
        }

        cEquipment.equipmentSystem.Weapon.projectileBehaviour.Destroy(pointer);

        return;
      }

      bullet.source.position += positionIncrement;

      if (bullet.distance >= bullet.range) cEquipment.equipmentSystem.Weapon.projectileBehaviour.Destroy(pointer);
    }
  }
}