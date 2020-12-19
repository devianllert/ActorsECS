using ActorsECS.UI.MenuSystem;
using UnityEngine;

namespace ActorsECS.UI.Menus
{
  public class MainMenu : SimpleMenu<MainMenu>
  {
    public void OnPlayPressed()
    {
      GameMenu.Show();
    }

    public override void OnBackPressed()
    {
      Application.Quit();
    }
  }
}