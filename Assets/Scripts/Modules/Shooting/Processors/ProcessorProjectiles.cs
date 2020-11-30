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
        ref var bullet = ref _bullets[pointer];

        bullet.distance += bullet.speed * delta;

        bullet.source.rotation = bullet.direction;

        var positionIncrement = bullet.source.forward * bullet.speed * delta;

        if (Physics.Raycast(bullet.source.position, positionIncrement.normalized, out var hit,
          positionIncrement.magnitude, LayerMask.GetMask("Enemy", "Environment")))
        {
          var actor = hit.transform.gameObject.GetComponent<Actor>();

          if (actor)
          {
            ref var cWeapon = ref _characters[0].ComponentEquipment();
            cWeapon.equipmentSystem.Weapon.Attack(_characters[0], actor.entity);

            DestroyBullet(bullet, pointer);

            continue;
          }

          DestroyBullet(bullet, pointer);

          continue;
        }

        bullet.source.position += positionIncrement;

        if (bullet.distance >= bullet.range) DestroyBullet(bullet, pointer);
      }
    }

    private void DestroyBullet(SegmentBullet bullet, int pointer)
    {
      var vfx = VFXManager.PlayVFX(VFXType.BulletHit, bullet.source.position);

      bullet.source.gameObject.Release(Pool.Entities);
      _bullets.RemoveAt(pointer);

      Layer.WaitFor(0.5f, () => vfx.Effect.gameObject.SetActive(false));
    }
  }
}