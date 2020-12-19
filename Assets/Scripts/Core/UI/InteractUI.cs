using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Core.UI
{
  public class InteractUI : Singleton<InteractUI>
  {
    public bool IsEnabled => gameObject.activeInHierarchy;

    public void ShowTooltip(Vector3 position)
    {
      var go = gameObject;

      go.SetActive(true);

      go.transform.position = position + new Vector3(0, 2f, 0);
    }

    public void HideTooltip()
    {
      gameObject.SetActive(false);
    }
  }
}