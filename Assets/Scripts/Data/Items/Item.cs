using ActorsECS.Core;
using Pixeye.Actors;
using UnityEditor;
using UnityEngine;

namespace ActorsECS.Data.Items
{
  /// <summary>
  ///   Base class of all items in the game. This is an abstract class and need to be inherited to specify behaviour.
  /// </summary>
  public abstract class Item : ScriptableObject
  {
    public string itemName;
    public Sprite itemSprite;
    public string description;
    public GameObject worldObjectPrefab;

    public virtual string GetDescription()
    {
      return description;
    }

    /// <summary>
    ///   Called by the inventory system when the object is "used" (double clicked)
    /// </summary>
    /// <param name="user">The CharacterDate that used that item</param>
    /// <returns>If it was actually used (allow the inventory to know if it can remove the object or not)</returns>
    public virtual bool UsedBy(ent user)
    {
      return false;
    }

    /// <summary>
    ///   Called by the pickup processor when the object is "picked up" (interaction button pressed)
    /// </summary>
    /// <param name="character">The character entity that picked up that item</param>
    /// <param name="loot">The loot entity that picked up</param>
    public virtual void Pickup(ent character, ent loot)
    {
      SFXManager.PlaySound(SFXManager.Use.Sound2D, new SFXManager.PlayData(){Clip = SFXManager.PickupSound});
    }

    /// <summary>
    ///   Called by the pickup processor when the object is "picked up" and we drop the previous item
    /// </summary>
    /// <param name="character">The character entity that dropped that item</param>
    public virtual void Drop(ent character)
    {
    }
  }

#if UNITY_EDITOR
  public class ItemEditor
  {
    private SerializedProperty _descriptionProperty;
    private SerializedProperty _iconProperty;

    private SerializedProperty _nameProperty;
    private SerializedObject _target;
    private SerializedProperty _worldObjectPrefabProperty;

    public void Init(SerializedObject target)
    {
      _target = target;

      _nameProperty = _target.FindProperty(nameof(Item.itemName));
      _iconProperty = _target.FindProperty(nameof(Item.itemSprite));
      _descriptionProperty = _target.FindProperty(nameof(Item.description));
      _worldObjectPrefabProperty = _target.FindProperty(nameof(Item.worldObjectPrefab));
    }

    public void GUI()
    {
      EditorGUILayout.PropertyField(_iconProperty);
      EditorGUILayout.PropertyField(_nameProperty);
      EditorGUILayout.PropertyField(_descriptionProperty, GUILayout.MinHeight(128));
      EditorGUILayout.PropertyField(_worldObjectPrefabProperty);
    }
  }
#endif
}