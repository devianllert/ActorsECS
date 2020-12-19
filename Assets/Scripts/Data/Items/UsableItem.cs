using System;
using System.Collections.Generic;
using System.Linq;
using ActorsECS.Data;
using Pixeye.Actors;
using UnityEditor;
using UnityEngine;

namespace ActorsECS.Data.Items
{
  /// <summary>
  ///   Describe an usable item. A usable item is an item that can be used in the inventory by double clicking on it.
  ///   When it is used, all the stored UsageEffects will be run, allowing to specify what that item does.
  ///   (e.g. a AddHealth effect will give health point back to the user)
  /// </summary>
  [CreateAssetMenu(menuName = "Game/Create/UsableItem")]
  public class UsableItem : Item
  {
    public List<UsageEffect> UsageEffects;

    public override bool UsedBy(ent character)
    {
      var wasUsed = false;

      foreach (var effect in UsageEffects) wasUsed |= effect.Use(character);

      return wasUsed;
    }

    public override string GetDescription()
    {
      var description = base.GetDescription();

      if (!string.IsNullOrWhiteSpace(description))
        description += "\n";
      else
        description = "";


      foreach (var effect in UsageEffects) description += effect.Description + "\n";

      return description;
    }

    public abstract class UsageEffect : ScriptableObject
    {
      public string Description;

      //return true if could be used, false otherwise.
      public abstract bool Use(ent character);
    }
  }


#if UNITY_EDITOR
  [CustomEditor(typeof(UsableItem))]
  public class UsableItemEditor : Editor
  {
    private List<string> m_AvailableUsageType;

    private ItemEditor m_ItemEditor;
    private UsableItem m_Target;
    private SerializedProperty m_UsageEffectListProperty;

    private void OnEnable()
    {
      m_Target = target as UsableItem;
      m_UsageEffectListProperty = serializedObject.FindProperty(nameof(UsableItem.UsageEffects));

      m_ItemEditor = new ItemEditor();
      m_ItemEditor.Init(serializedObject);

      var lookup = typeof(UsableItem.UsageEffect);
      m_AvailableUsageType = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
        .Select(type => type.Name)
        .ToList();
    }

    public override void OnInspectorGUI()
    {
      m_ItemEditor.GUI();

      var choice = EditorGUILayout.Popup("Add new Effect", -1, m_AvailableUsageType.ToArray());

      if (choice != -1)
      {
        var newInstance = CreateInstance(m_AvailableUsageType[choice]);

        AssetDatabase.AddObjectToAsset(newInstance, target);

        m_UsageEffectListProperty.InsertArrayElementAtIndex(m_UsageEffectListProperty.arraySize);
        m_UsageEffectListProperty.GetArrayElementAtIndex(m_UsageEffectListProperty.arraySize - 1).objectReferenceValue =
          newInstance;
      }

      Editor ed = null;
      var toDelete = -1;
      for (var i = 0; i < m_UsageEffectListProperty.arraySize; ++i)
      {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        var item = m_UsageEffectListProperty.GetArrayElementAtIndex(i);
        var obj = new SerializedObject(item.objectReferenceValue);

        CreateCachedEditor(item.objectReferenceValue, null, ref ed);

        ed.OnInspectorGUI();
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("-", GUILayout.Width(32))) toDelete = i;
        EditorGUILayout.EndHorizontal();
      }

      if (toDelete != -1)
      {
        var item = m_UsageEffectListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
        DestroyImmediate(item, true);

        //need to do it twice, first time just nullify the entry, second actually remove it.
        m_UsageEffectListProperty.DeleteArrayElementAtIndex(toDelete);
        m_UsageEffectListProperty.DeleteArrayElementAtIndex(toDelete);
      }

      serializedObject.ApplyModifiedProperties();
    }
  }
#endif
}