using ActorsECS.UI.MenuSystem;

namespace ActorsECS.UI.Menus
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