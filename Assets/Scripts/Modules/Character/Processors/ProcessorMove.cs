using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Common;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Character.Processors
{
  internal sealed class ProcessorMove : Processor, ITickFixed
  {
    [ExcludeBy(Tag.Roll)] private readonly Group<ComponentInput> _characters = default;

    public void TickFixed(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();
        ref var cMovementDirection = ref character.ComponentMovementDirection();
        ref var cMovement = ref character.ComponentMovement();
        var rigidbody = character.GetMono<Rigidbody>();

        var movement = new Vector3(cInput.Movement.x, 0, cInput.Movement.y);

        var speed = cMovementDirection.direction.x >= -0.1f ? cMovement.speed : cMovement.speed * 0.8f;

        var newPosition = rigidbody.transform.position + movement * speed * delta;

        rigidbody.MovePosition(newPosition);
      }
    }
  }
}