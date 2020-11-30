using UnityEditor;
using UnityEngine;

namespace ActorsECS.Data.Events.Editor
{
  [CustomEditor(typeof(GameEvent), true)]
  public class EventEditor : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      GUI.enabled = Application.isPlaying;

      var e = target as GameEvent;

      // ReSharper disable once PossibleNullReferenceException
      if (GUILayout.Button("Raise")) e.Raise();
    }
  }
}