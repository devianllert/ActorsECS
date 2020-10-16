using UnityEngine;

namespace Data.Collectables
{
  public enum PotionTypes
  {
    Health
  }
  
  [CreateAssetMenu(fileName = "Potions", menuName = "Data/Potions", order = 0)]
  public class Potion : ScriptableObject
  {
    public PotionTypes type = PotionTypes.Health;
  }
}