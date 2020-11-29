using ActorsECS.Data;
using ActorsECS.Modules.Common;
using Pixeye.Actors;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterWeaponController : MonoCached
{
  public Rig handsIK;
  public GameObject weaponHolder;

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
  }
}