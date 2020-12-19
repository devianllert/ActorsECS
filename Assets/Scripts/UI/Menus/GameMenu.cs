using ActorsECS.UI.MenuSystem;

namespace ActorsECS.UI.Menus
{
  public class GameMenu : SimpleMenu<GameMenu>
  {
    public override void OnBackPressed()
    {
      PauseMenu.Show();
    }
  }
}