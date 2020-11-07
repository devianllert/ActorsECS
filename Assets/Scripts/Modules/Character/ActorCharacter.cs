using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Common;
using Pixeye.Actors;

namespace ActorsECS.Modules.Character
{
  public class ActorCharacter : Actor
  {
    [FoldoutGroup("Components", true)]
    public ComponentInput componentInput;
    public ComponentHealth componentHealth;
    public ComponentRotation componentRotation;
    public ComponentMovement componentMovement;
    public ComponentMovementDirection componentMovementDirection;
    public ComponentWeapon componentWeapon;
    public ComponentRoll componentRoll;

    protected override void Setup()
    {
      componentWeapon.currentAmmo = componentWeapon.equippedWeapon.ammo;
      componentMovement.speed = 5f;

      entity.Set(componentInput);
      entity.Set(componentMovement);
      entity.Set(componentRotation);
      entity.Set(componentMovementDirection);
      entity.Set(componentWeapon);
      entity.Set(componentHealth);
      entity.Set(componentRoll);
    }
  }
}