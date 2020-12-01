using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Common;
using ActorsECS.Modules.Shooting.Components;
using Pixeye.Actors;

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
        
        bullet.weapon.projectileBehaviour.Tick(ref bullet, pointer);
      }
    }
  }
}