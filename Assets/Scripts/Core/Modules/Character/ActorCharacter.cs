using ActorsECS.Core.Modules.Character.Components;
using ActorsECS.Core.Modules.Character.Mono;
using ActorsECS.Core.Modules.Common;
using ActorsECS.Data.Items;
using Pixeye.Actors;

namespace ActorsECS.Core.Modules.Character
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