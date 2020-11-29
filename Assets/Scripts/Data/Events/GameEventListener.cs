using Pixeye.Actors;
using UnityEngine;
using UnityEngine.Events;

namespace ActorsECS.Data.Events
{
  public class GameEventListener : MonoCached
  {
    [Tooltip("Event to register with.")]
    public GameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent Response;

    public override void HandleEnable()
    {
      Event.RegisterListener(this);
    }

    public override void HandleDisable()
    {
      Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
      Response.Invoke();
    }
  }
}