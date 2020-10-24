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
        
        cweapon.fireTime -= delta;
        
        if (cinput.fired > 0 && !cweapon.reload && cweapon.fireTime <= 0)
        {
          cweapon.fireTime = 60 / cweapon.weapon.rate;
          
          ref var bullet = ref Layer.GetBuffer<SegmentBullet>().Add();
          bullet.position = transform.position + Vector3.up + transform.forward;
          bullet.speed = cweapon.weapon.speed;
          bullet.source = Obj.Create(Pool.Entities, "Prefabs/Bullet", bullet.position);
          bullet.distance = 0f;
          bullet.direction = character.ComponentRotation().rotation;
          bullet.direction.y += Random.Range(-0.05f, 0.05f);
          bullet.range = cweapon.weapon.range;
        }
      }
    }
  }
}
