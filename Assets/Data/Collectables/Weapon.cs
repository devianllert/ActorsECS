using UnityEngine;

namespace Data.Collectables
{
  public enum WeaponType
  {
    Rifle
  }
  
  [CreateAssetMenu(fileName = "Weapons", menuName = "Data/Weapons", order = 0)]
  public class Weapon : ScriptableObject
  {
    public WeaponType type = WeaponType.Rifle;
  }
}