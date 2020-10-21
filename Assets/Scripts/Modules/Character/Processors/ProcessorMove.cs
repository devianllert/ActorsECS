using ActorsECS.Modules.Character.Components;
using Pixeye.Actors;
using Unity.Mathematics;
using UnityEngine;

namespace ActorsECS.Modules.Character.Processors
{
  internal sealed class ProcessorMove : Processor, ITickFixed
  {
    private readonly Group<ComponentInput> _characters = default;
    
    public void TickFixed(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cinput = ref character.ComponentInput();
        ref var cmovementDirection = ref character.ComponentMovementDirection();
        ref var cmovement = ref character.ComponentMovement();
        var rigidbody = character.GetMono<Rigidbody>();
        
        var movement = new Vector3(cinput.movement.x, 0, cinput.movement.y);

        math.lerp(movement, 1, 0.5f);
        
        var speed = cmovementDirection.direction.x > 0 ? cmovement.speed : cmovement.speed * 0.6f;
        
        var newPosition = rigidbody.transform.position + movement * speed * delta;
        
        rigidbody.MovePosition(newPosition);
      }
    }
  }
}
