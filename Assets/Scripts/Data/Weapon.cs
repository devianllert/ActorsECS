using UnityEngine;

namespace ActorsECS.Data
{
  public enum WeaponType
  {
    Basic,
    Beam,
  }

  public enum BulletType
  {
    Bullet = 0,
    Laser = 1,
  }

  [CreateAssetMenu(fileName = "Weapons", menuName = "Data/Weapons", order = 0)]
  public class Weapon : ScriptableObject
  {
    public WeaponType type = WeaponType.Basic;
    public BulletType bullet = BulletType.Bullet; 
    public float rate;
    public float range;
    public float damage;
    public float ammo;
    public float speed;
    public float recoil;
  }
}