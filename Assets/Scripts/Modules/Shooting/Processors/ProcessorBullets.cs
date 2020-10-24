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
        
        var hit = GetMonster(
          out var entity,
          bullet.source.position,
          positionIncrement.normalized,
          positionIncrement.magnitude,
          LayerMask.GetMask("Enemy")
        );
        
        bullet.source.position += positionIncrement;

        if (hit)
        {
          entity.ComponentEnemy().health -= 1;
          
          Debug.Log(entity.ComponentEnemy().health);
          
          _bullets.RemoveAt(pointer);
          bullet.source.gameObject.Release(Pool.Entities);
        }
        
        if (bullet.distance >= bullet.range)
        {
          _bullets.RemoveAt(pointer);
          bullet.source.gameObject.Release(Pool.Entities);
        }
      }
    }
    
    public static bool GetMonster(out ent entity, Vector3 position, Vector3 direction, float distance, LayerMask mask)
    {
      if (Physics.Raycast(position, direction, out var hit, distance, mask))
      {
        entity = hit.transform.gameObject.GetEntity();
        
        if (entity.Has<ComponentEnemy>())
          return true;
      }

      entity = new ent(-1);
      
      return false;
    }
  }
}
