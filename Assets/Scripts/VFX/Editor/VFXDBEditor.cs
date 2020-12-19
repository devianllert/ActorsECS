using System.IO;
using ActorsECS.VFX;
using UnityEditor;
using UnityEngine;

namespace ActorsECS.Core.VFX.Editor
{
  [CustomEditor(typeof(VFXDatabase))]
  public class VFXDBEditor : UnityEditor.Editor
  {
    private SerializedProperty m_EntryProperty;
    private bool m_NeedGeneration;

    private void OnEnable()
    {
      m_EntryProperty = serializedObject.FindProperty(nameof(VFXDatabase.Entries));
      m_NeedGeneration = false;
    }

    private void OnDisable()
    {
      if (m_NeedGeneration)
      {
        Debug.Log("RegeneratingEnum");
        RegenerateEnum();
      }
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      if (GUILayout.Button("New Entry")) m_EntryProperty.InsertArrayElementAtIndex(m_EntryProperty.arraySize);

      var toDelete = -1;

      GUILayout.BeginHorizontal();
      GUILayout.Label("Name");
      GUILayout.Label("Prefab");
      GUILayout.Label("PoolSize");
      GUILayout.EndHorizontal();


      for (var i = 0; i < m_EntryProperty.arraySize; ++i)
      {
        GUILayout.BeginHorizontal();

        var entry = m_EntryProperty.GetArrayElementAtIndex(i);

        var nameProperty = entry.FindPropertyRelative(nameof(VFXDatabase.VFXDBEntry.Name));
        var objRef = entry.FindPropertyRelative(nameof(VFXDatabase.VFXDBEntry.Prefab));
        var poolSizeProp = entry.FindPropertyRelative(nameof(VFXDatabase.VFXDBEntry.PoolSize));

        EditorGUILayout.PropertyField(nameProperty, GUIContent.none);
        EditorGUILayout.PropertyField(objRef, GUIContent.none);
        EditorGUILayout.PropertyField(poolSizeProp, GUIContent.none);

        if (GUILayout.Button("-")) toDelete = i;

        GUILayout.EndHorizontal();
      }

      if (toDelete != -1) m_EntryProperty.DeleteArrayElementAtIndex(toDelete);

      if (serializedObject.hasModifiedProperties)
      {
        m_NeedGeneration = true;
        serializedObject.ApplyModifiedProperties();
      }
    }

    private void RegenerateEnum()
    {
      serializedObject.Update();

      //first we clean all null entry, as we don't want to generate an identifier for it
      for (var i = 0; i < m_EntryProperty.arraySize; ++i)
      {
        var objRef = m_EntryProperty.GetArrayElementAtIndex(i)
          .FindPropertyRelative(nameof(VFXDatabase.VFXDBEntry.Prefab));
        if (objRef.objectReferenceValue == null) m_EntryProperty.DeleteArrayElementAtIndex(i);
      }

      serializedObject.ApplyModifiedProperties();

      //then generate the script file
      var resultingEnum = "public enum VFXType\n{\n";
      for (var i = 0; i < m_EntryProperty.arraySize; ++i)
      {
        var nameProperty = m_EntryProperty.GetArrayElementAtIndex(i)
          .FindPropertyRelative(nameof(VFXDatabase.VFXDBEntry.Name));
        resultingEnum += $"\t{nameProperty.stringValue.Replace(' ', '_')}";

        if (i < m_EntryProperty.arraySize - 1)
          resultingEnum += ",\n";
      }

      resultingEnum += "\n}";

      var typeFile = AssetDatabase.FindAssets("t:Script VFXTypes");

      if (typeFile.Length != 1)
      {
        if (typeFile.Length == 0)
          Debug.LogError("You have no VFXTypes.cs file in the project!");
        else
          Debug.LogError("You have more than one VFXTypes.cs files in the project!");
      }
      else
      {
        var path = AssetDatabase.GUIDToAssetPath(typeFile[0]);
        File.WriteAllText(path.Replace("Assets", Application.dataPath), resultingEnum);

        AssetDatabase.Refresh();
      }
    }

    [MenuItem("Assets/Create/VFX/VFXDatabase", priority = -800)]
    private static void CreateAssetDB()
    {
      var existingDb = AssetDatabase.FindAssets("t:VFXDatabase");
      var selectionPath = "";

      if (existingDb.Length > 0)
      {
        Debug.LogError("A VFXDatabase already exists.");
        selectionPath = AssetDatabase.GUIDToAssetPath(existingDb[0]);
      }
      else
      {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (path == "")
          path = "Assets";
        else if (Path.GetExtension(path) != "")
          path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");

        var newDb = CreateInstance<VFXDatabase>();
        var assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/VFXDatabase.asset");
        AssetDatabase.CreateAsset(newDb, assetPath);
        selectionPath = assetPath;
      }

      Selection.activeObject = AssetDatabase.LoadAssetAtPath<VFXDatabase>(selectionPath);
    }
  }
}