using ActorsECS.Core.UI.MenuSystem;

namespace ActorsECS.Core.UI.Menus
{
  public class PauseMenu : SimpleMenu<PauseMenu>
  {
    public void OnQuitPressed()
    {
      Hide();
      Destroy(gameObject);
    }
  }
}