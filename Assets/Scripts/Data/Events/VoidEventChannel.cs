using UnityEngine;
using UnityEngine.Events;

namespace ActorsECS.Data.Events
{
  [CreateAssetMenu(menuName = "Game/Events/Void Event Channel")]
  public class VoidEventChannel : ScriptableObject
  {
    public UnityAction OnEventRaised;

    public void RaiseEvent() => OnEventRaised?.Invoke();
  }
}