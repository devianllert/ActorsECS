using Pixeye.Actors;
using UnityEditor;
using UnityEngine;

namespace ActorsECS.Data
{
  /// <summary>
  /// Base class of all items in the game. This is an abstract class and need to be inherited to specify behaviour.
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

    public virtual void Pickup(ent character, ent loot) {}
  }

#if UNITY_EDITOR
  public class ItemEditor
  {
    SerializedObject _target;

    SerializedProperty _nameProperty;
    SerializedProperty _iconProperty;
    SerializedProperty _descriptionProperty;
    SerializedProperty _worldObjectPrefabProperty;
    
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