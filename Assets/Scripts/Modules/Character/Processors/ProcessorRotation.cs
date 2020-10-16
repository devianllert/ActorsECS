using Modules.Character.Components;
using Pixeye.Actors;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Character.Processors
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

      Physics.Raycast(screenRay.origin, screenRay.GetPoint(200), out var hit);
      
      foreach (var character in _characters)
      {
        ref var cinput = ref character.ComponentInput();
        ref var crotation = ref character.ComponentRotation();
        ref var cmovementDirection = ref character.ComponentMovementDirection();
        var rigidbody = character.GetMono<Rigidbody>();
        
        var closestHitPosition = hit.point - rigidbody.transform.position;
        closestHitPosition.y = 0;

        var newRotation = Quaternion.LookRotation(closestHitPosition, Vector3.up);

        var desiredDirection = cameraForward * cinput.movement.y + cameraRight * cinput.movement.x;

        var movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);
        
        var forw = math.dot(movement, math.mul(crotation.rotation, math.forward()));
        var stra = math.dot(movement, math.mul(crotation.rotation, math.right()));
        
        cmovementDirection.direction = new Vector2(forw, stra);
        
        crotation.rotation = newRotation;
        
        rigidbody.MoveRotation(newRotation);
      }
    }
  }
}
