using ActorsECS.Modules.Enemy.Components;
using ActorsECS.Modules.Shooting.Components;
using Pixeye.Actors;
using UnityEngine;

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
          if (hit.transform.gameObject.TryGetComponent<Actor>(out var actor))
          {
            ref var cenemy = ref actor.entity.ComponentEnemy();
            
            cenemy.health -= bullet.damage;
            
            Debug.Log(cenemy.health);
            
            DestroyBullet(bullet, pointer);
            
            return;
          }
          
          DestroyBullet(bullet, pointer);
        }

        bullet.source.position += positionIncrement;

        if (bullet.distance >= bullet.range)
        {
          DestroyBullet(bullet, pointer);

          return;
        }
      }
    }

    public void DestroyBullet(SegmentBullet bullet, int pointer)
    {
      _bullets.RemoveAt(pointer);
      bullet.source.gameObject.Release(Pool.Entities);
    }
  }
}
