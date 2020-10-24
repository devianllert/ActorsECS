using UnityEngine;

namespace ActorsECS.Modules.Shooting.Components
{
  public struct SegmentLaser
  {
    public Transform source;
    public Vector3 position;
    public Quaternion direction;
    public float range;
  }
}