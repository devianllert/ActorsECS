using ActorsECS.Core.Modules.Character.Components;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Core.Modules.Character.Processors
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
        ref var cMovementDirection = ref character.ComponentMovementDirection();
        var cAnimator = character.GetMono<Animator>();

        cAnimator.SetBool(Running, !cMovementDirection.direction.Equals(Vector2.zero));

        cAnimator.SetFloat(Forward, cMovementDirection.direction.x);
        cAnimator.SetFloat(Strafe, cMovementDirection.direction.y);
      }
    }
  }
}