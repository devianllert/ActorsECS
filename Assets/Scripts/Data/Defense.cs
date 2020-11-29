namespace ActorsECS.Data
{
  public static class Defense
  {
    public static int CalculateElementalDamage(int resistance, int damage)
    {
      return (1 - resistance / 100) * damage;
    }

    public static int CalculatePhysicalDamageReduction(int armorRating, int damage)
    {
      return armorRating / (armorRating + 10 * damage);
    }

    public static int CalculateDamageAfterDR(int armorRating, int damage)
    {
      return 10 * damage * damage / (armorRating + 10 * damage);
    }
  }
}