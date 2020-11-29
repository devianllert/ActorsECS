using ActorsECS.Data;
using ActorsECS.Modules.Common;
using Pixeye.Actors;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterWeaponController : MonoCached
{
  public Rig handsIK;
  public GameObject weaponHolder;

  public Transform rightHandGrip;
  public Transform leftHandGrip;

  public Transform GetWeaponObject()
  {
    return weaponHolder.transform.GetChild(2);
  }

  public void SetupWeaponModel(WeaponItem weapon)
  {
    if (GetComponent<Actor>().entity.Get<ComponentWeapon>().equippedWeapon) Destroy(GetWeaponObject().gameObject);

    Obj.Create(weapon.worldObjectPrefab, weaponHolder.transform);

    weaponHolder.transform.localPosition = Vector3.zero;
    weaponHolder.transform.localRotation = Quaternion.identity;

    handsIK.weight = 1f;
    var animatorController = GetComponent<Animator>();
    var overrides = animatorController.runtimeAnimatorController as AnimatorOverrideController;
    animatorController.SetLayerWeight(1, 1f);

    overrides["EmptyWeaponAnimation"] = weapon.handGripAnimation;
  }
  
  #if UNITY_EDITOR
  [ContextMenu("Save Weapon Position")]
  void SaveWeaponPosition()
  {
    var recorder = new GameObjectRecorder(gameObject);
    
    recorder.BindComponentsOfType<Transform>(weaponHolder.gameObject, false);
    recorder.BindComponentsOfType<Transform>(leftHandGrip.gameObject, false);
    recorder.BindComponentsOfType<Transform>(rightHandGrip.gameObject, false);
    
    recorder.TakeSnapshot(0f);
    
    recorder.SaveToClip(GetComponent<Actor>().entity.Get<ComponentWeapon>().equippedWeapon.handGripAnimation);
    
    UnityEditor.AssetDatabase.SaveAssets();
  }
  #endif
}