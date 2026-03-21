using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FormationPreview))]
public class FormationPreviewEditor: Editor
{
    // SO의 데이터를 수정하기 위한 SerializedObject와 Property
    private SerializedObject soData;
    private SerializedProperty pointsProperty;

    private void OnEnable()
    {
        // 에디터가 활성화될 때 (오브젝트를 클릭했을 때) 초기화
        FormationPreview preview = (FormationPreview)target;
        if (preview.FormationData != null)
        {
            InitSerializedObject();
        }
    }

    /*SO를 새로 가져와서 직렬화 한다.
     일종의 새로고침이라고 생각하면 편하다.*/
    private void InitSerializedObject()
    {
        FormationPreview preview = (FormationPreview)target;
        // SO를 직렬화 객체로 가져옵니다.
        soData = new SerializedObject(preview.FormationData);
        // SO 내부의 "points" 리스트 필드를 찾습니다.
        pointsProperty = soData.FindProperty("points");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // target은 Object 형태로 전달돼서 형변환 필요
        FormationPreview preview = (FormationPreview)target;

        soData.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("--- Formation Data Edit ---", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(pointsProperty, true);

        if (GUILayout.Button("Add Point", GUILayout.Height(25)))
        {
            Undo.RecordObject(preview.FormationData, "Add Point");
            preview.FormationData.points.Add(Vector2.zero);
            EditorUtility.SetDirty(preview.FormationData);
        }

        if(GUILayout.Button("Remove Point", GUILayout.Height(25)))
        {
            Undo.RecordObject(preview.FormationData, "Remove Point");
            if(preview.FormationData.points.Count > 0)
            {
                preview.FormationData.points.RemoveAt(preview.FormationData.points.Count - 1);
                EditorUtility.SetDirty(preview.FormationData);
            }
        }

        if (GUILayout.Button("Reload points", GUILayout.Height(25)))
        {
            InitSerializedObject();
        }

        soData.ApplyModifiedProperties();
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
