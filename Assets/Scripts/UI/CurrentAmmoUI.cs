using Pixeye.Actors;
using TMPro;

namespace ActorsECS.UI
{
  public class CurrentAmmoUI : Singleton<CurrentAmmoUI>
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