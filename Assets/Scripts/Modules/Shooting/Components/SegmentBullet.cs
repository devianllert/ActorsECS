using UnityEngine;

namespace ActorsECS.Modules.Shooting.Components
{
  public struct SegmentBullet
  {
    public Transform source;
    public Vector3 position;
    public float speed;
    public float distance;
    public Quaternion direction;
    public float range;
    public float damage;
  }
}