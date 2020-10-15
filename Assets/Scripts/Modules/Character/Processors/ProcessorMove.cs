using Modules.Character.Components;
using Pixeye.Actors;
using UnityEngine;

namespace Modules.Character.Processors
{
  internal sealed class ProcessorMove : Processor, ITickFixed
  {
    private readonly Group<ComponentInput> _characters = default;
    
    public void TickFixed(float delta)
    {
      foreach (var character in _characters)
      {
        ref var inputData = ref character.CharacterInputData();
        var rigidbody = character.GetMono<Rigidbody>();
        
        var movement = new Vector3(inputData.movement.x, 0, inputData.movement.y);

        var newPosition = rigidbody.transform.position + movement * 5f * delta;
        
        rigidbody.MovePosition(newPosition);
      }
    }
  }
}
