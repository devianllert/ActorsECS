using ActorsECS.Modules.Character.Components;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Character.Processors
{
  internal sealed class ProcessorAnimation : Processor, ITick
  {
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Forward = Animator.StringToHash("Forward");
    private static readonly int Strafe = Animator.StringToHash("Strafe");

    private readonly Group<ComponentInput> _characters = default;
    
    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cmovementDirection = ref character.ComponentMovementDirection();
        var canimator = character.GetMono<Animator>();

        canimator.SetBool(Running, !cmovementDirection.direction.Equals(Vector2.zero));
        
        canimator.SetFloat(Forward, cmovementDirection.direction.y);
        canimator.SetFloat(Strafe, cmovementDirection.direction.x);
      }
    }
  }
}
