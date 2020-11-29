using Pixeye.Actors;
using UnityEngine.UI;

namespace ActorsECS.UI
{
  public class ReloadUI : MonoCached
  {
    private Image _filledImage;

    private float elapsedTime;

    private bool isReloading;
    private float reloadTime;

    private void Update()
    {
      UpdateReloadState();
    }

    protected override void OnEnable()
    {
      _filledImage = GetComponentInChildren<Image>();
      _filledImage.gameObject.SetActive(false);
    }

    public void StartReload(float time)
    {
      isReloading = true;

      _filledImage.gameObject.SetActive(true);
      _filledImage.fillAmount = 0;
      elapsedTime = 0;
      reloadTime = time;
    }

    private void UpdateReloadState()
    {
      if (!isReloading) return;

      if (_filledImage.fillAmount >= 1)
      {
        isReloading = false;

        _filledImage.gameObject.SetActive(false);
      }

      elapsedTime += Time.deltaTime;
      var reloadElapsed = elapsedTime / reloadTime;

      _filledImage.fillAmount = reloadElapsed;
    }
  }
}