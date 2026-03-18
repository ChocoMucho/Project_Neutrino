using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FormationPreview))]
public class FormationPreviewEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // target은 Object 형태로 전달돼서 형변환 필요
        FormationPreview preview = (FormationPreview)target;

        if(GUILayout.Button("Add Point"))
        {
            Undo.RecordObject(preview.FormationData, "Add Point");
            preview.FormationData.points.Add(Vector2.zero);
            EditorUtility.SetDirty(preview.FormationData);
        }

        if(GUILayout.Button("Remove Point"))
        {
            Undo.RecordObject(preview.FormationData, "Remove Point");
            if(preview.FormationData.points.Count > 0)
            {
                preview.FormationData.points.RemoveAt(preview.FormationData.points.Count - 1);
                EditorUtility.SetDirty(preview.FormationData);
            }
        }
    }

    void OnSceneGUI()
    {
        FormationPreview preview = (FormationPreview)target;
        if (preview.FormationData.points == null) return;

        Handles.color = Color.green;    

        for (int i = 0; i < preview.FormationData.points.Count; ++i)
        {
            Vector2 currentPoint = preview.FormationData.points[i];

            Handles.Label(currentPoint + Vector2.up * 0.2f, $"Point {i}");

            Vector2 newPos = Handles.FreeMoveHandle
                (
                currentPoint,
                preview.HandleSize,
                Vector3.zero,
                Handles.DotHandleCap
                );

            if (GUI.changed)
            {
                Undo.RecordObject(preview.FormationData, "Move Point");
                preview.FormationData.points[i] = newPos;
                EditorUtility.SetDirty(preview.FormationData);
            }
        }
    }
}
