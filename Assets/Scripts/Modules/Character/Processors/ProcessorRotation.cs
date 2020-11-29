using ActorsECS.Modules.Character.Components;
using Pixeye.Actors;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ActorsECS.Modules.Character.Processors
{
  internal sealed class ProcessorRotation : Processor, ITickFixed
  {
    [ExcludeBy(Tag.Roll)] private readonly Group<ComponentInput> _characters = default;

    private static Camera Camera => Camera.main;
    private static Mouse Mouse => Mouse.current;

    public void TickFixed(float delta)
    {
      var looking = Mouse.position.ReadValue();
      var transform = Camera.transform;

      var cameraForward = transform.forward;
      var cameraRight = transform.right;

      var screenRay = Camera.ScreenPointToRay(looking);

      var plane = new Plane(Vector3.up, 0);

      plane.Raycast(screenRay, out var dist);

      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();
        ref var cRotation = ref character.ComponentRotation();
        ref var cAim = ref character.ComponentAim();
        ref var cMovementDirection = ref character.ComponentMovementDirection();
        var rigidbody = character.GetMono<Rigidbody>();

        var closestHitPosition = screenRay.GetPoint(dist) - rigidbody.transform.position;
        closestHitPosition.y = 0;
        var newRotation = quaternion.LookRotation(closestHitPosition, Vector3.up);

        var desiredDirection = cameraForward * cInput.Movement.y + cameraRight * cInput.Movement.x;

        var movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);

        var forw = math.dot(movement, math.mul(cRotation.rotation, math.forward()));
        var stra = math.dot(movement, math.mul(cRotation.rotation, math.right()));

        cMovementDirection.direction = new Vector2(forw, stra);

        rigidbody.MoveRotation(newRotation);

        cAim.point = closestHitPosition;
        cRotation.rotation = newRotation;
        cRotation.faceDirection = rigidbody.transform.forward;
      }
    }
  }
}