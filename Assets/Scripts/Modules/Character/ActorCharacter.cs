using ActorsECS.Modules.Character.Components;
using Pixeye.Actors;

namespace ActorsECS.Modules.Character
{
  public class ActorCharacter : Actor
  {
    [FoldoutGroup("Components", true)] public ComponentInput componentInput;
    public ComponentRotation componentRotation;
    public ComponentMovement componentMovement;
    public ComponentMovementDirection componentMovementDirection;
    public ComponentWeapon componentWeapon;

    protected override void Setup()
    {
      componentWeapon.isReloading = false;
      componentWeapon.currentAmmo = componentWeapon.equippedWeapon.ammo;
      componentMovement.speed = 5f;

      entity.Set(componentInput);
      entity.Set(componentMovement);
      entity.Set(componentRotation);
      entity.Set(componentMovementDirection);
      entity.Set(componentWeapon);
    }
  }
}