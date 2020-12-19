using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Core.Modules.Shooting.Components
{
  public struct SegmentLaser
  {
    public ent owner;
    public LayerMask mask;
    
    public Transform source;
    public Vector3 position;
    public Quaternion direction;
    public float range;
  }
}