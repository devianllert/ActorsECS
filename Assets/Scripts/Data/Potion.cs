using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Data
{
  public enum PotionTypes
  {
    Health
  }
  
  [CreateAssetMenu(fileName = "Potions", menuName = "Data/Potions", order = 0)]
  public class Potion : ScriptableObject, IUsable
  {
    public PotionTypes type = PotionTypes.Health;
    public float restoreAmount = 0;
    public float cooldown = 2f;

    public void Use(ent entity)
    {
      // ref var health = ref entity.Get<SomeComponent>();
      // health += restoreAmount;
    }
  }
}