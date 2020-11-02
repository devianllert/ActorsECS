using ActorsECS.Modules.Character.Components;
using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules.Character.Processors
{
  internal sealed class ProcessorLootCollisions : Processor, ITickFixed
  {
    private ent _lootEntity = new ent(-1);

    private Collider[] _lootColliders = new Collider[1];

    private readonly Group<ComponentInput> _characters = default;

    public void TickFixed(float delta)
    {
      foreach (var character in _characters)
      {
        var col = character.GetMono<CapsuleCollider>();
        var transform = character.GetMono<Rigidbody>().transform;

        var (top, bottom) = GetCapsuleBounds(col, transform);

        var hits = Physics.OverlapCapsuleNonAlloc(top, bottom, col.radius + 2f, _lootColliders,
          LayerMask.GetMask("Collectable"));

        if (hits == 0)
        {
          if (_lootEntity.id != -1) _lootEntity.RemoveAll(Tag.Lootable);

          _lootEntity = new ent(-1);

          continue;
        }

        var newEntity = _lootColliders[0].gameObject.GetEntity();

        if (_lootEntity.id == newEntity.id) return;

        if (!newEntity.Has(Tag.Lootable)) newEntity.Set(Tag.Lootable);

        if (_lootEntity.id != -1 && _lootEntity.Has(Tag.Lootable)) _lootEntity.RemoveAll(Tag.Lootable);

        _lootEntity = newEntity;
      }
    }
    
    private (Vector3, Vector3) GetCapsuleBounds(CapsuleCollider collider, Transform transform)
    {
      var center = transform.TransformPoint(collider.center);
      var size = transform.TransformVector(collider.radius, collider.height, collider.radius);
      var radius = size.x;
      var height = size.y;

      var bottom = new Vector3(center.x, center.y - height / 2 + radius, center.z);
      var top = new Vector3(center.x, center.y + height / 2 - radius, center.z);

      return (top, bottom);
    }
  }
}