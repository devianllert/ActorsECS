using Pixeye.Actors;
using TMPro;

namespace ActorsECS.UI
{
  public class TotalAmmoUI : MonoCached
  {
    private TMP_Text _totalAmmo;

    protected override void OnEnable()
    {
      _totalAmmo = GetComponent<TMP_Text>();
    }

    public void UpdateTotalAmmo(int value)
    {
      _totalAmmo.text = value.ToString();
    }
  }
}