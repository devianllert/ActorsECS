using ActorsECS.Data.Items;
using ActorsECS.Modules.Common;
using Pixeye.Actors;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ActorsECS.Modules.Character
{
  public class CharacterWeaponController : MonoCached
  {
    public Rig handsIK;
    public GameObject weaponHolder;

    public Transform rightHandGrip;
    public Transform leftHandGrip;

    public bool HasSetupedWeapon()
    {
      return weaponHolder.transform.childCount > 2;
    }
    
    public Transform GetWeaponObject()
    {
      return weaponHolder.transform.GetChild(2);
    }

    public Transform FindProjectilePoint(Transform weaponObject)
    {
      return weaponObject.Find("ProjectilePoint");
    }

    public void SetupWeaponModel(WeaponItem weapon)
    {
      if (HasSetupedWeapon()) Destroy(GetWeaponObject().gameObject);

      var weaponObject = Obj.Create(weapon.worldObjectPrefab, weaponHolder.transform);

      weaponHolder.transform.localPosition = Vector3.zero;
      weaponHolder.transform.localRotation = Quaternion.identity;

      handsIK.weight = 1f;
      var animatorController = GetComponent<Animator>();
      var overrides = animatorController.runtimeAnimatorController as AnimatorOverrideController;
      animatorController.SetLayerWeight(1, 1f);

      overrides["EmptyWeaponAnimation"] = weapon.handGripAnimation;

      GetComponent<Actor>().entity.ComponentWeapon().projectilePoint = FindProjectilePoint(weaponObject);
    }
    
    public void SetupWeaponModel()
    {
      if (HasSetupedWeapon()) Destroy(GetWeaponObject().gameObject);

      weaponHolder.transform.localPosition = Vector3.zero;
      weaponHolder.transform.localRotation = Quaternion.identity;

      handsIK.weight = 0f;
      var animatorController = GetComponent<Animator>();
      var overrides = animatorController.runtimeAnimatorController as AnimatorOverrideController;
      animatorController.SetLayerWeight(1, 0);

      overrides["EmptyWeaponAnimation"] = null;
    }

#if UNITY_EDITOR
    [ContextMenu("Save Weapon Position")]
    private void SaveWeaponPosition()
    {
      var recorder = new GameObjectRecorder(gameObject);

      recorder.BindComponentsOfType<Transform>(weaponHolder.gameObject, false);
      recorder.BindComponentsOfType<Transform>(leftHandGrip.gameObject, false);
      recorder.BindComponentsOfType<Transform>(rightHandGrip.gameObject, false);

      recorder.TakeSnapshot(0f);

      recorder.SaveToClip(GetComponent<Actor>().entity.ComponentEquipment().equipmentSystem.Weapon.handGripAnimation);

      AssetDatabase.SaveAssets();
    }
#endif
  }
}