using Assets.Scripts;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletsTrack))]
public class CollectableTrackEditor : Editor
{
    SerializedProperty m_Track;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var button = (IApplyButton)target;
        if (GUILayout.Button("Apply"))
            button.Apply();
    }
}

