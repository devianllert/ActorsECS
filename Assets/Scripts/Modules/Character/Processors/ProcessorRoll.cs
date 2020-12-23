using ActorsECS.Modules.Character.Components;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Character.Processors
{
  internal sealed class ProcessorRoll : Processor, ITick, ITickFixed
  {
    private static readonly int Roll = Animator.StringToHash("Roll");

    private readonly Group<ComponentInput> _characters = default;

    [GroupBy(Tag.Roll)] private readonly Group<ComponentInput> _rolledCharacters = default;

    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();
        ref var cRoll = ref character.ComponentRoll();

        if (cInput.Roll && !character.Has(Tag.Roll) && cRoll.elapsedCooldown <= 0f) character.Set(Tag.Roll);

        if (cRoll.elapsedCooldown > 0) cRoll.elapsedCooldown -= delta;
      }
    }

    public void TickFixed(float delta)
    {
      foreach (var rolledCharacter in _rolledCharacters)
      {
        ref var cRoll = ref rolledCharacter.ComponentRoll();
        ref var cRotation = ref rolledCharacter.ComponentRotation();
        var cRigidbody = rolledCharacter.GetMono<Rigidbody>();

        var rollSpeed = cRoll.distance / cRoll.duration;

        var rollVelocity = cRoll.elapsedDuration <= 0.35
          ? cRotation.faceDirection * 3
          : cRotation.faceDirection * rollSpeed;

        cRoll.elapsedDuration -= delta;

        cRigidbody.velocity = new Vector3(rollVelocity.x, cRigidbody.velocity.y, rollVelocity.z);

        if (cRoll.elapsedDuration <= 0)
        {
          cRigidbody.velocity = new Vector3(0f, cRigidbody.velocity.y, 0f);

          rolledCharacter.Remove(Tag.Roll);
        }
      }
    }

    public override void HandleEcsEvents()
    {
      foreach (var character in _rolledCharacters.added)
      {
        ref var cRoll = ref character.ComponentRoll();
        var cAnimator = character.GetMono<Animator>();
        var cRigidbody = character.GetMono<Rigidbody>();

        cRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        cAnimator.SetBool(Roll, true);

        cRoll.elapsedCooldown = cRoll.cooldown;
        cRoll.elapsedDuration = cRoll.duration;

        SetIgnoreCollisionsWithEnemies(true);
      }

      foreach (var character in _rolledCharacters.removed)
      {
        var cAnimator = character.GetMono<Animator>();
        var cRigidbody = character.GetMono<Rigidbody>();

        cRigidbody.constraints -= RigidbodyConstraints.FreezeRotationY;

        cAnimator.SetBool(Roll, false);

        SetIgnoreCollisionsWithEnemies(false);
      }
    }

    private void SetIgnoreCollisionsWithEnemies(bool ignore)
    {
      var playerMask = LayerMask.NameToLayer("Player");
      var enemiesMask = LayerMask.NameToLayer("Enemy");

      Physics.IgnoreLayerCollision(playerMask, enemiesMask, ignore);
    }
  }
}