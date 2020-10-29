using System.Collections;
using ActorsECS.Modules.Enemy.Components;
using ActorsECS.Modules.Shooting.Components;
using ActorsECS.VFX;
using Pixeye.Actors;
using UnityEngine;
using UnityEngine.VFX;
using VFXManager = ActorsECS.VFX.VFXManager;

namespace ActorsECS.Modules.Shooting.Processors
{
  internal sealed class ProcessorBullets : Processor, ITick
  {
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
            ref var cenemy = ref actor.entity.ComponentEnemy();
            
            cenemy.health -= bullet.damage;
            
            DestroyBullet(bullet, pointer);
            
            continue;
          }
          
          DestroyBullet(bullet, pointer);
          
          continue;
        }

        bullet.source.position += positionIncrement;

        if (bullet.distance >= bullet.range)
        {
          DestroyBullet(bullet, pointer);
        }
      }
    }

    public void DestroyBullet(SegmentBullet bullet, int pointer)
    {
      var vfx = VFXManager.PlayVFX(VFXType.BulletHit, bullet.source.position);

      _bullets.RemoveAt(pointer);
      bullet.source.gameObject.Release(Pool.Entities);

      Layer.WaitFor(0.5f, () => vfx.Effect.gameObject.SetActive(false));
    }
  }
}
