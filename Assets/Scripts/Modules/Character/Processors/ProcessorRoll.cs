using ActorsECS.Modules.Character.Components;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Character.Processors
{
  internal sealed class ProcessorRoll : Processor, ITick, ITickFixed
  {
    private static readonly int Roll = Animator.StringToHash("Roll");
    
    [GroupBy(Tag.Roll)]
    private readonly Group<ComponentInput> _rolledCharacters = default;

    private readonly Group<ComponentInput> _characters = default;
    
    public override void HandleEcsEvents()
    {
      foreach (var character in _rolledCharacters.added)
      {
        ref var cRoll = ref character.ComponentRoll();
        var cAnimator = character.GetMono<Animator>();
        var cRigidbody = character.GetMono<Rigidbody>();

        cRigidbody.freezeRotation = true;
        
        cAnimator.SetBool(Roll, true);

        cRoll.elapsedCooldown = cRoll.cooldown;
        cRoll.elapsedDuration = cRoll.duration;
      }
      
      foreach (var character in _rolledCharacters.removed)
      {
        var cAnimator = character.GetMono<Animator>();
        var cRigidbody = character.GetMono<Rigidbody>();

        cRigidbody.freezeRotation = false;

        cAnimator.SetBool(Roll, false);
      }
    }

    public void TickFixed(float delta)
    {
      foreach (var rolledCharacter in _rolledCharacters)
      {
        ref var cRoll = ref rolledCharacter.ComponentRoll();
        ref var cRotation = ref rolledCharacter.ComponentRotation();
        var cRigidbody = rolledCharacter.GetMono<Rigidbody>();

        var rollVelocity = cRoll.elapsedDuration <= 0.3
          ? cRotation.faceDirection * cRoll.speed / 2
          : cRotation.faceDirection * cRoll.speed;

        cRoll.elapsedDuration -= delta;

        cRigidbody.velocity = new Vector3(rollVelocity.x, cRigidbody.velocity.y, rollVelocity.z);
        
        if (cRoll.elapsedDuration <= 0)
        {
          cRigidbody.velocity = new Vector3(0f,cRigidbody.velocity.y, 0f);

          rolledCharacter.Remove(Tag.Roll);
        };
      }
    }

    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();
        ref var cRoll = ref character.ComponentRoll();

        if (!character.Has(Tag.Roll) && cInput.Roll && cRoll.elapsedCooldown <= 0f)
        {
          character.Set(Tag.Roll);
        }

        if (cRoll.elapsedCooldown > 0)
        {
          cRoll.elapsedCooldown -= delta;
        }
      }
    }
  }
}
