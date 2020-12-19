using Pixeye.Actors;
using TMPro;

namespace ActorsECS.Core.UI
{
  public class CurrentAmmoUI : MonoCached
  {
    private TMP_Text _currentAmmo;

    protected override void OnEnable()
    {
      _currentAmmo = GetComponent<TMP_Text>();
    }

    public void UpdateCurrentAmmo(int value)
    {
      _currentAmmo.text = value.ToString();
    }
  }
}