using ActorsECS.Core.UI.MenuSystem;

namespace ActorsECS.Core.UI.Menus
{
  public class GameMenu : SimpleMenu<GameMenu>
  {
    public override void OnBackPressed()
    {
      PauseMenu.Show();
    }
  }
}