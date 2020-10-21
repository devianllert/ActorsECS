using UnityEngine;

namespace ActorsECS.Data
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