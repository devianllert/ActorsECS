using Game.Source;
using Modules.Character.Components;
using Pixeye.Actors;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Character.Processors
{
  internal sealed class ProcessorRotation : Processor, ITick
  {
    private readonly Group<ComponentInput, ComponentRotation> _characters = default;
    
    private static Camera Camera => Camera.main;
    private static Mouse Mouse => Mouse.current;
    
    public void Tick(float delta)
    {
      var looking = Mouse.position.ReadValue();
            
      var screenRay = Camera.ScreenPointToRay(looking);

      Physics.Raycast(screenRay.origin, screenRay.GetPoint(200), out var hit);
      
      foreach (var character in _characters)
      {
        ref var crotation = ref character.ComponentRotation();
        var rigidbody = character.GetMono<Rigidbody>();
        
        var closestHitPosition = hit.point - rigidbody.transform.position;
        closestHitPosition.y = 0;

        var newRotation = Quaternion.LookRotation(closestHitPosition, Vector3.up);

        crotation.rotation = newRotation;
        
        rigidbody.MoveRotation(newRotation);
      }
    }
  }
}
