using ActorsECS.Core.UI.MenuSystem;
using UnityEngine;

namespace ActorsECS.Core.UI.Menus
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