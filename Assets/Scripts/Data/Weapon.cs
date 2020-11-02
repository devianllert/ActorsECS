using ActorsECS.VFX;
using UnityEngine;

namespace ActorsECS.Data
{
  public enum BulletType
  {
    Bullet = 0,
    Laser = 1
  }

  [CreateAssetMenu(fileName = "Weapons", menuName = "Data/Weapons", order = 0)]
  public class Weapon : ScriptableObject
  {
    [Header("Bullet Type")]
    public BulletType bulletType = BulletType.Bullet;
   
    [Space]
    [Header("Weapon Data")]
    public float rateOfFire;
    public float range;
    public float damage;
    public int ammo;
    public float speed;
    public float reloadTime;

    [Space]
    [Header("Bullet Data")]
    public string bulletPrefabName;
    public VFXType HitVFX;
  }
}