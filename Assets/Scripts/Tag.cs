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
    [TagField(categoryName = "Condition")] public const int Roll = 13;

    [TagField(categoryName = "Enemy")] public const int Patrol = 30;
    [TagField(categoryName = "Enemy")] public const int Attack = 31;
    [TagField(categoryName = "Enemy")] public const int Chase = 32;

    [TagField(categoryName = "Affliction")]
    public const int Afflicted = 100;

    [TagField(categoryName = "Affliction")]
    public const int Stun = 101;

    [TagField(categoryName = "Affliction")]
    public const int Burn = 102;

    [TagField(categoryName = "Affliction")]
    public const int Acid = 103;

    [TagField(categoryName = "Affliction")]
    public const int Chill = 104;

    #endregion
  }
}