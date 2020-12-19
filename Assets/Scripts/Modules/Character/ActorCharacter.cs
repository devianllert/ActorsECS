using ActorsECS.Data.Items;
using ActorsECS.Modules.Character.Components;
using ActorsECS.Modules.Character.Mono;
using ActorsECS.Modules.Common;
using Pixeye.Actors;

namespace ActorsECS.Modules.Character
{
  public class ActorCharacter : Actor
  {
    [FoldoutGroup("Components", true)]
    public ComponentMovement componentMovement;
    public ComponentRoll componentRoll;
    public ComponentStats componentStats;
    public ComponentEquipment componentEquipment;
    private ComponentAim componentAim;
    private ComponentInput componentInput;
    private ComponentMovementDirection componentMovementDirection;
    private ComponentRotation componentRotation;

    protected override void Setup()
    {
      componentStats.statSystem.stats.health = 100;
      componentStats.statSystem.Init(entity);
      componentEquipment.equipmentSystem.Init(entity);

      componentEquipment.equipmentSystem.OnEquiped = item =>
      {
        var weaponController = entity.GetMono<CharacterWeaponController>();
        weaponController.SetupWeaponModel(item as WeaponItem);
      };
      
      entity.Set(componentInput);
      entity.Set(componentMovement);
      entity.Set(componentRotation);
      entity.Set(componentMovementDirection);
      entity.Set(componentRoll);
      entity.Set(componentStats);
      entity.Set(componentEquipment);
      entity.Set(componentAim);
    }
  }
}