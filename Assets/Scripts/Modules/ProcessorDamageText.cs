using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Modules
{
  internal sealed class ProcessorDamageText : Processor, ITick
  {
    private Buffer<SegmentDamageText> _texts => Layer.GetBuffer<SegmentDamageText>();

    
    public void Tick(float delta)
    {
      foreach (var pointer in _texts)
      {
        ref var text = ref _texts[pointer];

        if (!(Camera.main is null))
        {
          var transform = text.source.transform;
          transform.localRotation = new Quaternion(Camera.main.transform.localRotation.x, 0, 0, transform.localRotation.w);
        }

        if (text.startTime + 0.4f < UnityEngine.Time.time)
        {
          text.source.gameObject.Release(Pool.Entities);
          _texts.RemoveAt(pointer);
        }
      }
    }
  }
}
