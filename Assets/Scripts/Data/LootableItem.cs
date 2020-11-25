using Pixeye.Actors;
using UnityEngine;

namespace ActorsECS.Data
{
  [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
  public class LootableItem : ScriptableObject
  {
    public Item item;
    
    public void Pickup(ent character, ent loot) => item.Pickup(character, loot);
  }
}