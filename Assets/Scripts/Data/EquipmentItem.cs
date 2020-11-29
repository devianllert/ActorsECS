﻿using System.Collections.Generic;
using System.Linq;
using Pixeye.Actors;
using UnityEditor;
using UnityEngine;

namespace ActorsECS.Data
{
  [CreateAssetMenu(fileName = "EquipmentItem", menuName = "Data/Create/Equipment", order = 0)]
  public class EquipmentItem : Item
  {
    public enum EquipmentSlot
    {
      Head,
      Torso,
      Legs,
      Feet,
      Accessory
    }
    
    public abstract class EquippedEffect : ScriptableObject
    {
      public string description;
      
      //return true if could be used, false otherwise.
      public abstract void Equipped(ent character);
      public abstract void Removed(ent character);
  
      public virtual string GetDescription()
      {
        return description;
      }
    }
    
    public EquipmentSlot slot;
    
    public List<EquippedEffect> equippedEffects;
    
    public void EquippedBy(ent character)
    {
      foreach (var effect in equippedEffects)
        effect.Equipped(character);
    }
    
    public void UnequippedBy(ent character)
    {
      foreach (var effect in equippedEffects)
        effect.Removed(character);
    }
  }

#if UNITY_EDITOR
  [CustomEditor(typeof(EquipmentItem))]
  public class EquipmentItemEditor : Editor
  {
    EquipmentItem _target;
    
    ItemEditor _itemEditor;

    List<string> _availableEquipEffectType;
    SerializedProperty _equippedEffectListProperty;

    SerializedProperty _slotProperty;

    void OnEnable()
    {
      _target = target as EquipmentItem;
      _equippedEffectListProperty = serializedObject.FindProperty(nameof(EquipmentItem.equippedEffects));

      _slotProperty = serializedObject.FindProperty(nameof(EquipmentItem.slot));
        
      _itemEditor = new ItemEditor();
      _itemEditor.Init(serializedObject);

      var lookup = typeof(EquipmentItem.EquippedEffect);
      _availableEquipEffectType = System.AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
        .Select(type => type.Name)
        .ToList();
    }

    public override void OnInspectorGUI()
    {
      _itemEditor.GUI();

      EditorGUILayout.PropertyField(_slotProperty);
        
      int choice = EditorGUILayout.Popup("Add new Effect", -1, _availableEquipEffectType.ToArray());

      if (choice != -1)
      {
        var newInstance = ScriptableObject.CreateInstance(_availableEquipEffectType[choice]);
            
        AssetDatabase.AddObjectToAsset(newInstance, target);
            
        _equippedEffectListProperty.InsertArrayElementAtIndex(_equippedEffectListProperty.arraySize);
        _equippedEffectListProperty.GetArrayElementAtIndex(_equippedEffectListProperty.arraySize - 1).objectReferenceValue = newInstance;
      }

      Editor ed = null;
      var toDelete = -1;
      for (var i = 0; i < _equippedEffectListProperty.arraySize; ++i)
      {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        var item = _equippedEffectListProperty.GetArrayElementAtIndex(i);           
        var obj = new SerializedObject(item.objectReferenceValue);

        Editor.CreateCachedEditor(item.objectReferenceValue, null, ref ed);
            
        ed.OnInspectorGUI();
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("-", GUILayout.Width(32)))
        {
          toDelete = i;
        }
        EditorGUILayout.EndHorizontal();
      }

      if (toDelete != -1)
      {
        var item = _equippedEffectListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
        DestroyImmediate(item, true);
            
        //need to do it twice, first time just nullify the entry, second actually remove it.
        _equippedEffectListProperty.DeleteArrayElementAtIndex(toDelete);
        _equippedEffectListProperty.DeleteArrayElementAtIndex(toDelete);
      }

      serializedObject.ApplyModifiedProperties();
    }
  }
#endif
}