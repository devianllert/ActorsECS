using ActorsECS.Modules.Character.Components;
using Pixeye.Actors;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ActorsECS.Modules.Character.Processors
{
  internal sealed class ProcessorRotation : Processor, ITick
  {
    private readonly Group<ComponentInput> _characters = default;

    private static Camera Camera => Camera.main;
    private static Mouse Mouse => Mouse.current;

    public void Tick(float delta)
    {
      var looking = Mouse.position.ReadValue();
      var transform = Camera.transform;

      var cameraForward = transform.forward;
      var cameraRight = transform.right;

      var screenRay = Camera.ScreenPointToRay(looking);

      Physics.Raycast(screenRay, out var dist);

      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();
        ref var cRotation = ref character.ComponentRotation();
        ref var cMovementDirection = ref character.ComponentMovementDirection();
        var rigidbody = character.GetMono<Rigidbody>();

        var closestHitPosition = dist.point - rigidbody.transform.position;
        closestHitPosition.y = 0;

        var newRotation = quaternion.LookRotation(closestHitPosition, Vector3.up);

        var desiredDirection = cameraForward * cInput.Movement.y + cameraRight * cInput.Movement.x;

        var movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);

        var forw = math.dot(movement, math.mul(cRotation.rotation, math.forward()));
        var stra = math.dot(movement, math.mul(cRotation.rotation, math.right()));

        cMovementDirection.direction = new Vector2(forw, stra);

        cRotation.rotation = newRotation;

        rigidbody.MoveRotation(newRotation);
      }
    }
  }
}