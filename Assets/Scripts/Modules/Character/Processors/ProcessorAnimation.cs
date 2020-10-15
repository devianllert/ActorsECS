using Modules.Character.Components;
using Pixeye.Actors;
using Unity.Mathematics;
using UnityEngine;


namespace Game.Source
{
  sealed class ProcessorAnimation : Processor, ITick
  {
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Forward = Animator.StringToHash("Forward");
    private static readonly int Strafe = Animator.StringToHash("Strafe");

    private readonly Group<ComponentInput, ComponentRotation> _characters = default;
    
    private static Camera Camera => Camera.main;
    
    public void Tick(float delta)
    {
      var transform = Camera.transform;

      var cameraForward = transform.forward;
      var cameraRight = transform.right;

      foreach (var character in _characters)
      {
        var cInputData = character.CharacterInputData();
        var crotation = character.ComponentRotation();
        var canimator = character.GetMono<Animator>();

        canimator.SetBool(Running, !cInputData.movement.Equals(Vector2.zero));
        
        var desiredDirection = cameraForward * cInputData.movement.y + cameraRight * cInputData.movement.x;

        var movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);
        
        var forw = math.dot(movement, math.mul(crotation.rotation, math.forward()));
        var stra = math.dot(movement, math.mul(crotation.rotation, math.right()));
        
        canimator.SetFloat(Forward, forw);
        canimator.SetFloat(Strafe, stra);
      }
    }
  }
}
