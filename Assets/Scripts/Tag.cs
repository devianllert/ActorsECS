using Pixeye.Actors;

namespace ActorsECS
{
  public static class Tag
  {
    #region Actions 0 - 1000

    [TagField(categoryName = "Loot")] public const int Lootable = 1;
    
    [TagField(categoryName = "Condition")] public const int Dead = 10;
    [TagField(categoryName = "Condition")] public const int Reload = 11;
    [TagField(categoryName = "Condition")] public const int Invulnerable = 12;
    
    [TagField(categoryName = "Affliction")] public const int Stun = 100;
    [TagField(categoryName = "Affliction")] public const int Burn = 101;
    [TagField(categoryName = "Affliction")] public const int Acid = 102;
    [TagField(categoryName = "Affliction")] public const int Chill = 103;

    #endregion
  }
}