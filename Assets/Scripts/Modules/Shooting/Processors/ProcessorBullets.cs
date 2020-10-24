using ActorsECS.Modules.Shooting.Components;
using Pixeye.Actors;

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
        bullet.source.position += bullet.source.forward * bullet.speed * delta;
        
        if (bullet.distance >= bullet.range)
        {
          _bullets.RemoveAt(pointer);
          bullet.source.gameObject.Release(Pool.Entities);
        }
      }
    }
  }
}
